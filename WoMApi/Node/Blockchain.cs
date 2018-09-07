using log4net;
using NBitcoin;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using WoMInterface.Game.Interaction;
using WoMInterface.Tool;

namespace WoMApi.Node
{
    public class Blockchain
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const decimal mogwaiCost = 1.0m;

        private const decimal txFee = 0.0001m;

        private static Blockchain instance;

        private RestClient client;

        public static Blockchain Instance => instance ?? (instance = new Blockchain());

        public Dictionary<int, string> BlockHashDict { get; }

        private Blockchain()
        {
            client = new RestClient("https://cristof.crabdance.com/mogwai/");

            BlockHashDict = new Dictionary<int, string>();
        }

        public void GetBlockHashCache(int height)
        {
            
        }

        public Block GetBlock(string hash)
        {
            var request = new RestRequest("getblock/{hash}", Method.GET);
            request.AddUrlSegment("hash", hash);
            IRestResponse<Block> blockResponse = client.Execute<Block>(request);
            return blockResponse.Data;
        }

        public string GetBlockHash(int height)
        {
            var request = new RestRequest("getblockhash/{height}", Method.GET);
            request.AddUrlSegment("height", height);
            IRestResponse blockResponse = client.Execute(request);
            return blockResponse.Content;
        }

        public double GetBlockCount()
        {
            var request = new RestRequest("getblockcount", Method.GET);
            IRestResponse<double> blockResponse = client.Execute<double>(request);
            return blockResponse.Data;
        }

        public decimal GetBalance(string address)
        {
            var request = new RestRequest("getbalance/{address}", Method.GET);
            request.AddUrlSegment("address", address);
            IRestResponse<decimal> blockResponse = client.Execute<decimal>(request);
            return blockResponse.Data;
        }

        public List<UnspentTx> GetUnspent(int minConf, int maxConf, string address)
        {
            var request = new RestRequest("listunspent/{minConf}/{maxConf}/{address}", Method.GET);
            request.AddUrlSegment("minConf", minConf);
            request.AddUrlSegment("maxConf", maxConf);
            request.AddUrlSegment("address", address);
            IRestResponse<List<UnspentTx>> blockResponse = client.Execute<List<UnspentTx>>(request);
            return blockResponse.Data;
        }

        public string SendRawTransaction(string rawTransaction)
        {
            var request = new RestRequest("sendrawtransaction/{hex}", Method.GET);
            request.AddUrlSegment("hex", rawTransaction);
            IRestResponse blockResponse = client.Execute(request);
            return blockResponse.Content;
        }

        public List<TxDetail> ListTransaction(string address)
        {
            //:height/:numblocks
            var request = new RestRequest("listtransactions/{address}", Method.GET);
            request.AddUrlSegment("address", address);
            IRestResponse<List<TxDetail>> blockResponse = client.Execute<List<TxDetail>>(request);
            return blockResponse.Data;

        }

        public bool BurnMogs(MogwaiKeys mogwaiKey, string toaddress, decimal burnMogs, decimal txFee)
        {
            var unspentTxList = GetUnspent(6, 9999999, mogwaiKey.Address);
            var unspentAmount = unspentTxList.Sum(p => p.Amount);

            if (unspentAmount < (burnMogs + txFee))
            {
                _log.Debug($"Address hasn't enough funds {unspentAmount} to burn that amount of mogs {(burnMogs + txFee)}!");
                return false;
            }

            // create transaction
            Transaction tx = mogwaiKey.CreateTransaction(unspentTxList, unspentAmount, toaddress, burnMogs, txFee);

            _log.Info($"signedRawTx: {tx.ToHex()}");

            var answer = SendRawTransaction(tx.ToHex());

            _log.Info($"sendRawTx: {answer}");

            return true;
        }

        public Dictionary<double, Shift> GetShifts(string mirroraddress, out bool openShifts)
        {
            var result = new Dictionary<double, Shift>();
        
            List<TxDetail> allTxs = ListTransaction(mirroraddress);

            var incUnconfTx = allTxs.Where(p => p.Address == mirroraddress && p.Category == "send").OrderBy(p => p.Blocktime).ThenBy(p => p.Blockindex).ToList();
            var validTx = incUnconfTx.Where(p => p.Confirmations > 0).ToList();
            openShifts = incUnconfTx.Count() > validTx.Count();

            var pubMogAddressHex = HexHashUtil.ByteArrayToString(Base58Encoding.Decode(mirroraddress));

            bool creation = false;
            int lastBlockHeight = 0;
            //foreach (var tx in validTx)
            //{
            //    decimal amount = Math.Abs(tx.Amount);
            //    if (!creation && amount < mogwaiCost)
            //        continue;

            //    creation = true;

            //    var block = GetBlock(tx.Blockhash);

            //    if (lastBlockHeight != 0 && lastBlockHeight + 1 < block.Height)
            //    {
            //        // add small shifts
            //        //if (TryGetBlockHashes(lastBlockHeight + 1, block.Height, null, out Dictionary<int, string> blockHashes))
            //        //{
            //        //    foreach (var blockHash in blockHashes)
            //        //    {
            //        //        result.Add(blockHash.Key, new Shift(result.Count(), pubMogAddressHex, blockHash.Key, blockHash.Value));
            //        //    }
            //        //}
            //    }

            //    lastBlockHeight = block.Height;

            //    result.Add(block.Height, new Shift(result.Count(), tx.Blocktime, pubMogAddressHex, block.Height, tx.Blockhash, tx.Blockindex, tx.Txid, amount, Math.Abs(tx.Fee + txFee)));
            //}

            //// add small shifts
            //if (creation && TryGetBlockHashes(lastBlockHeight + 1, (int)mogwaiService.GetBlockCount(), null, out Dictionary<int, string> finalBlockHashes))
            //{
            //    foreach (var blockHash in finalBlockHashes)
            //    {
            //        result.Add(blockHash.Key, new Shift(result.Count(), pubMogAddressHex, blockHash.Key, blockHash.Value));
            //    }
            //}

        //    //result.ForEach(p => Console.WriteLine(p.ToString()));
            return result;
        }

    }
}
