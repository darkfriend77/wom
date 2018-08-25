using BitcoinLib.Requests.CreateRawTransaction;
using BitcoinLib.Requests.SignRawTransaction;
using BitcoinLib.Responses;
using BitcoinLib.Services.Coins.Mogwaicoin;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game;
using WoMInterface.Tool;

namespace WoMInterface.Node
{
    public class Blockchain
    {
        internal enum BoundState
        {
            BOUND, WAIT, NONE
        }

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const decimal mogwaiCost = 1.0m;

        private const decimal txFee = 0.0001m;

        IMogwaicoinService mogwaiService;

        private static Blockchain instance;

        public static Blockchain Instance => instance == null ? instance = new Blockchain() : instance;

        private CachingService cachingService = new CachingService(); 

        public Blockchain(string daemonUrl, string rpcUsername, string rpcPassword, string walletPassword)
        {
            mogwaiService = new MogwaicoinService(daemonUrl, rpcUsername, rpcPassword, walletPassword, 10);
        }

        private Blockchain()
        {
            mogwaiService = new MogwaicoinService(
                ConfigurationManager.AppSettings["daemonUrl"],
                ConfigurationManager.AppSettings["rpcUsername"],
                ConfigurationManager.AppSettings["rpcPassword"],
                ConfigurationManager.AppSettings["walletPassword"],
                10);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Exit()
        {
            Console.WriteLine("Persisting current cached informations.");
            cachingService.Persist();
            Console.WriteLine("Stoping rpc service.");
            mogwaiService.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Cache(bool clear, bool initial)
        {
            var progress = new ProgressBar(60);

            uint blockcount = mogwaiService.GetBlockCount();

            int maxBlockCached = clear ? 0 : cachingService.Cache.MaxBlockHash;

            for (int i = maxBlockCached + 1; i < blockcount; i++)
            {
                if (initial)
                {
                    progress.Update(i * 100 / blockcount);
                }

                string blockHash = mogwaiService.GetBlockHash(i);
                cachingService.Cache.BlockHashDict.Add(i, blockHash);
            }

            if (initial)
            {
                progress.Update(100);
                cachingService.Persist();
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void CacheStats()
        {
            Console.WriteLine($"Blockhashes: {cachingService.Cache.BlockHashDict.Count()}");
            Console.WriteLine($"BlockHeight: {cachingService.Cache.MaxBlockHash} [curr:{mogwaiService.GetBlockCount()}]");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal GetInfoResponse GetInfo()
        {
            return mogwaiService.GetInfo();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="mogwai"></param>
        /// <returns></returns>
        internal BoundState TryGetMogwai(string address, out Mogwai mogwai)
        {
            mogwai = null;

            if (!TryGetMogwaiAddress(address, out string mogwaiAddress))
            {
                return BoundState.NONE;
            }

            BoundState boundState = IsMogwaiBound(address, mogwaiAddress, out List<Shift> shifts);

            if (boundState == BoundState.BOUND)
            {
                mogwai = new Mogwai(address, shifts);
            }

            return boundState;
        }

        /// <summary>
        /// TODO: This is a workaround function please remove once fixed.
        /// </summary>
        /// <param name="fromBlockHeight"></param>
        /// <param name="pattern"></param>
        /// <param name="blockHashes"></param>
        /// <returns></returns>
        public bool TryGetBlockHashes(int fromBlockHeight, int toBlockHeight, string[] pattern, out Dictionary<int, string> blockHashes)
        {
            blockHashes = new Dictionary<int, string>();

            uint blockcount = mogwaiService.GetBlockCount();

            if (cachingService.Cache.MaxBlockHash < blockcount)
            {
                Cache(false, false);
            }

            string blockHash;
            for (int i = fromBlockHeight; i < toBlockHeight; i++)
            {
                if (!cachingService.Cache.BlockHashDict.TryGetValue(i, out blockHash))
                {
                    return false;
                }
                if (pattern == null || StringHelpers.StringContainsStringFromArray(blockHash, pattern))
                {
                    blockHashes.Add(i, blockHash);
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mogwaiAddress"></param>
        /// <returns></returns>
        private List<Shift> GetShifts(string mogwaiAddress, out bool openShifts)
        {
            var result = new List<Shift>();

            var allTxs = new List<ListTransactionsResponse>();

            List<ListTransactionsResponse> listTxs = null;
            int index = 0;
            int chunkSize = 50;
            while (listTxs == null || listTxs.Count > 0)
            {
                listTxs = mogwaiService.ListTransactions("", chunkSize, index);
                allTxs.AddRange(listTxs);
                index += chunkSize;
            }

            var incUnconfTx = allTxs.Where(p => p.Address == mogwaiAddress && p.Category == "send").OrderBy(p => p.Time).ThenBy(p=> p.BlockIndex);
            var validTx = incUnconfTx.Where(p => p.Confirmations > 0);
            openShifts = incUnconfTx.Count() > validTx.Count();

            var pubMogAddressHex = HexHashUtil.ByteArrayToString(Base58Encoding.Decode(mogwaiAddress));

            bool creation = false;
            int lastBlockHeight = 0;
            foreach (var tx in validTx)
            {
                decimal amount = Math.Abs(tx.Amount);
                if (!creation && amount < mogwaiCost)
                    continue;
                
                creation = true;

                var block = mogwaiService.GetBlock(tx.BlockHash);

                if(lastBlockHeight != 0 && lastBlockHeight + 1 < block.Height)
                {
                    // add small shifts
                    if ( TryGetBlockHashes(lastBlockHeight + 1, block.Height, null, out Dictionary<int,string> blockHashes))
                    {
                        foreach(var blockHash in blockHashes)
                        {
                            result.Add(new SmallShift()
                            {
                                AdHex = pubMogAddressHex,
                                BkHex = blockHash.Value,
                                Height = blockHash.Key
                            });
                        }
                    }
                }

                lastBlockHeight = block.Height;

                result.Add(new Shift() {
                    Time = tx.Time,
                    AdHex = pubMogAddressHex,
                    Height = block.Height,
                    BkHex = tx.BlockHash,
                    BkIndex = tx.BlockIndex,
                    TxHex = tx.TxId,
                    Amount = amount
                });
            }

            // add small shifts
            if (creation && TryGetBlockHashes(lastBlockHeight + 1, (int) mogwaiService.GetBlockCount(), null, out Dictionary<int, string> finalBlockHashes))
            {
                foreach (var blockHash in finalBlockHashes)
                {
                    result.Add(new SmallShift()
                    {
                        BkHex = blockHash.Value,
                        Height = blockHash.Key
                    });
                }
            }

            //result.ForEach(p => Console.WriteLine(p.ToString()));
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        internal bool EvolveMogwai(string address)
        {
            if (!TryGetMogwaiAddress(address, out string mogwaiAddress))
            {
                return false;
            }

            if (IsMogwaiBound(address, mogwaiAddress, out List<Shift> shifts) != BoundState.BOUND)
            {
                return false;
            }

            Console.WriteLine($"Evolving mogwai now!");

            var burned = BurnMogs(address, mogwaiAddress, mogwaiCost, txFee);

            return burned;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        internal bool BindMogwai(string address)
        {
            if (!TryGetMogwaiAddress(address, out string mogwaiAddress))
            {
                return false;
            }

            //Console.WriteLine($"{address} --> {mogwaiAddress}");

            if (IsMogwaiBound(address, mogwaiAddress, out List<Shift> shifts) != BoundState.NONE)
            {
                Console.WriteLine("Mogwai already exists or is in creation process!");
                return false;
            }

            Console.WriteLine($"Starting mogwai creation now!");

            var burned = BurnMogs(address, mogwaiAddress, mogwaiCost, txFee);

            return burned;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromaddress"></param>
        /// <param name="listUnspent"></param>
        /// <returns></returns>
        internal decimal UnspendFunds(string fromaddress, out List<ListUnspentResponse> listUnspent)
        {
            listUnspent = mogwaiService.ListUnspent(6, 9999999, new List<string> { fromaddress });
            var unspentAmount = listUnspent.Sum(p => p.Amount);
            return unspentAmount;
        }

        /// <summary>
        /// Function to burn mogs
        /// </summary>
        /// <param name="fromaddress"></param>
        /// <param name="toaddress"></param>
        /// <param name="burnMogs"></param>
        /// <param name="txFee"></param>
        /// <returns></returns>
        private bool BurnMogs(string fromaddress, string toaddress, decimal burnMogs, decimal txFee)
        {
            var unspentAmount = UnspendFunds(fromaddress, out List<ListUnspentResponse> listUnspent);
            if (unspentAmount < (burnMogs + txFee))
            {
                Console.WriteLine($"Address hasn't enough funds {unspentAmount} to burn that amount of mogs {(burnMogs + txFee)}!");
                return false;
            }

            // create raw transaction
            var rawTxRequest = new CreateRawTransactionRequest();

            // adding all unspent txs
            listUnspent.ForEach(p => {
                rawTxRequest.AddInput(new CreateRawTransactionInput() { TxId = p.TxId, Vout = p.Vout });
            });

            rawTxRequest.AddOutput(new CreateRawTransactionOutput() { Address = toaddress, Amount = burnMogs });

            // check if we need a changeaddress
            if ((unspentAmount - burnMogs) > txFee)
            {
                //Console.WriteLine($"Adding change output {unspentAmount - burnMogs - txFee}");
                rawTxRequest.AddOutput(new CreateRawTransactionOutput() { Address = fromaddress, Amount = unspentAmount - burnMogs - txFee });
            }

            var rawTx = mogwaiService.CreateRawTransaction(rawTxRequest);
            Console.WriteLine($"rawTx: {rawTx}");

            var signedRawTx = mogwaiService.SignRawTransaction(new SignRawTransactionRequest(rawTx));
            Console.WriteLine($"signedRawTx: {signedRawTx.Hex}");

            var sendRawTx = mogwaiService.SendRawTransaction(signedRawTx.Hex, false);
            Console.WriteLine($"sendRawTx: {sendRawTx}");

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mogwaiAddress"></param>
        /// <param name="shifts"></param>
        /// <returns></returns>
        internal BoundState IsMogwaiBound(string address, string mogwaiAddress, out List<Shift> shifts)
        {
            shifts = GetShifts(mogwaiAddress, out bool openShifts);
            // no shifts found
            if (shifts.Count == 0)
            {
                //Console.WriteLine("No mogwai bound to this address, 0 transactions found, try to bind a mogwai first!");
                return openShifts ? BoundState.WAIT : BoundState.NONE;
            }
            return BoundState.BOUND;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="mogwaiAddress"></param>
        /// <returns></returns>
        private bool TryGetMogwaiAddress(string address, out string mogwaiAddress)
        {
            var mirrorAddress = mogwaiService.MirrorAddress(address);
            if (!mirrorAddress.IsMine || !mirrorAddress.IsValid || !mirrorAddress.IsMirAddrValid)
            {
                //Console.WriteLine($"Haven't found a valid mogwaiwaddress for {address}!");
                mogwaiAddress = null;
                return false;
            }

            mogwaiAddress = mirrorAddress.MirAddress;
            //Console.WriteLine($"mogwaiaddress: {mogwaiAddress}");
            return true;
        }

        internal bool NewMogwaiAddress(out string mogwaiAddress, int tryes = 10)
        {
            mogwaiAddress = null;
            List<string> listPoolAddresses;

            for (int i = 0; i < tryes; i++)
            {
                listPoolAddresses = mogwaiService.GetAddressesByAccount("Pool");

                if (listPoolAddresses.Count == 0)
                {
                    mogwaiService.GetNewAddress("Pool");
                }

                // get new created address
                listPoolAddresses = mogwaiService.GetAddressesByAccount("Pool");

                if (TryGetMogwaiAddress(listPoolAddresses[0], out mogwaiAddress))
                {
                    mogwaiService.SetAccount(listPoolAddresses[0], "Mogwai");
                    return true;
                }
                else
                {
                    mogwaiService.SetAccount(listPoolAddresses[0], "");
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ValidMogwaiAddresses()
        {
            var listAddresses = mogwaiService.GetAddressesByAccount("Mogwai");
            Dictionary<string, string> result = new Dictionary<string, string>();
            listAddresses.ForEach(p => {
                if (TryGetMogwaiAddress(p, out string mogwaiAddress))
                {
                    result.Add(p, mogwaiAddress);
                }
                else
                {
                    mogwaiService.SetAccount(p, "");
                }
            });
            return result;
        }

        internal void Print(string address)
        {
            var mirrorAddress = mogwaiService.MirrorAddress(address);


            if (mirrorAddress.IsValid && mirrorAddress.IsMine)
            {
                Console.WriteLine($"MirrorPubKey: {mirrorAddress.MirKey}");
                Console.WriteLine($"IsValidMirrorKey: {mirrorAddress.MirKeyValid}");
                Console.WriteLine($"IsValidMirrorAddress: {mirrorAddress.IsMirAddrValid}");
                Console.WriteLine($"MirrorAddress: {mirrorAddress.MirAddress}");

                if (mirrorAddress.IsMirAddrValid)
                {
                    Console.WriteLine($"MirrorAddress (Decode): {HexHashUtil.ByteArrayToString(Base58Encoding.Decode(mirrorAddress.MirAddress))}");
                }


                if (false)
                {

                    var mirrTransactions = mogwaiService.ListMirrorTransactions(mirrorAddress.MirAddress);
                    mirrTransactions.ForEach(t =>
                    {
                        var txId = t.TxId;
                        var amount = t.Amount;
                        var fullTx = mogwaiService.GetRawTransaction(txId, 1);
                        var vins = fullTx.Vin;
                        var vouts = fullTx.Vout;
                        var vout = vouts[0];
                        var totAmount = vouts.ToList().Sum(p => p.Value);
                        List<string> inputAddresses = new List<string>();
                        foreach (var vin in vins)
                        {
                            var prevFullTx = mogwaiService.GetRawTransaction(vin.TxId, 1);
                            var prevTotAmount = prevFullTx.Vout.ToList().Sum(p => p.Value);
                            var retAmount = prevTotAmount - totAmount;
                            var Fee = mogwaiService.GetTransaction(prevFullTx.TxId).Fee;
                            foreach (var prevVout in prevFullTx.Vout)
                            {
                                if (prevVout.Value != retAmount)
                                {
                                    Console.WriteLine($"{retAmount}");
                                    var sendAddress = prevVout.ScriptPubKey.Addresses[0];
                                    if (!inputAddresses.Contains(sendAddress))
                                    {
                                        inputAddresses.Add(sendAddress);
                                    }
                                }
                            }

                            //var prevVout = prevFullTx.Vout.Where(vo => vo.Value == totAmount).FirstOrDefault();
                            //if (prevVout != null)
                            //{
                            //    var sendAddress = prevVout.ScriptPubKey.Addresses[0];
                            //    if (!inputAddresses.Contains(sendAddress))
                            //    {
                            //        inputAddresses.Add(sendAddress);
                            //   }
                            //}
                        }

                        Console.WriteLine($"{vout.ScriptPubKey.Addresses[0]} : Tot: {totAmount} - Amount: {vout.Value}");
                        inputAddresses.ForEach(ia =>
                        {
                            Console.WriteLine($" - {ia}");
                        });

                    });

                }

            }
        }

    }
}
