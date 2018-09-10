using log4net;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using WoMWallet.Tool;
using WoMFramework.Game.Interaction;
using WoMFramework.Game.Model;

namespace WoMWallet.Node
{
    public enum MogwaiKeysState
    {
        NONE, WAIT, READY, CREATE, BOUND
    }

    public class MogwaiKeys
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExtKey extkey;

        private readonly Network network;

        private readonly PubKey pubKey;

        private readonly PubKey mirrorPubKey;

        public string Address => pubKey.GetAddress(network).ToString();

        public string MirrorAddress => mirrorPubKey?.GetAddress(network).ToString();

        public bool HasMirrorAddress => mirrorPubKey != null;

        public Mogwai Mogwai { get; set; }

        public decimal Balance { get; set; } = 0.0000m;

        public Dictionary<double, Shift> Shifts { get; set; }

        private MogwaiKeysState oldMogwaiKeysState = MogwaiKeysState.NONE;
        private MogwaiKeysState mogwaiKeysState = MogwaiKeysState.NONE;
        public MogwaiKeysState MogwaiKeysState
        {
            get { return mogwaiKeysState; }
            set
            {
                oldMogwaiKeysState = mogwaiKeysState;
                mogwaiKeysState = value;
            }
        }

        public MogwaiKeys(ExtKey extkey, Network network)
        {
            this.extkey = extkey;
            this.network = network;
            this.pubKey = extkey.PrivateKey.PubKey;
            if (TryMirrorPubKey(extkey.PrivateKey.PubKey, out PubKey mirrorPubKey))
            {
                this.mirrorPubKey = mirrorPubKey;
            }
        }

        public string GetEncryptedSecretWif()
        {
            return extkey.PrivateKey.GetEncryptedBitcoinSecret(extkey.ToString(network), network).ToWif();
        }

        private bool TryMirrorPubKey(PubKey pubKey, out PubKey mirrorPubKey)
        {
            var pubKeyStr = Helpers.ByteArrayToString(pubKey.ToBytes());
            var mirrorPubKeyStr =
                   pubKeyStr.Substring(0, 8)
                 + pubKeyStr.Substring(8, 2)
                 + Helpers.ReverseString(pubKeyStr.Substring(10, 54))
                 + pubKeyStr.Substring(64, 2);
            var mirrorPubKeyBytes = Helpers.StringToByteArray(mirrorPubKeyStr);
            try
            {
                mirrorPubKey = new PubKey(mirrorPubKeyBytes, false);
                return false;
            }
            catch (Exception)
            {
                //Console.WriteLine(ex.InnerException);
                mirrorPubKey = new PubKey(mirrorPubKeyBytes, true);
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async void Update()
        {
            await Task.Run(() =>
            {
                Balance = Blockchain.Instance.GetBalance(Address);
                if (HasMirrorAddress)
                {
                    Shifts = Blockchain.Instance.GetShifts(MirrorAddress);
                    if (Shifts.Count > 0)
                    {
                        Mogwai = new Mogwai(Address, Shifts);
                    }
                }

                if (Mogwai != null)
                {
                    MogwaiKeysState = MogwaiKeysState.BOUND;
                }
                else if(Balance > 1.0001m && MogwaiKeysState != MogwaiKeysState.CREATE)
                {
                    MogwaiKeysState = MogwaiKeysState.READY;
                }
                else if(Balance < 1.0001m && MogwaiKeysState != MogwaiKeysState.WAIT)
                {
                    MogwaiKeysState = MogwaiKeysState.NONE;
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unspentTxList"></param>
        /// <param name="unspentAmount"></param>
        /// <param name="toaddress"></param>
        /// <param name="burnMogs"></param>
        /// <param name="txFee"></param>
        /// <returns></returns>
        public Transaction CreateTransaction(List<UnspentTx> unspentTxList, decimal unspentAmount, string toaddress, decimal burnMogs, decimal txFee)
        {
            // creating a new transaction
            Transaction tx = Transaction.Create(network);

            // adding all unspent txs to input
            unspentTxList.ForEach(p =>
            {
                tx.AddInput(new TxIn
                {
                    PrevOut = new OutPoint(new uint256(p.Txid), p.Vout),
                    ScriptSig = pubKey.GetAddress(network).ScriptPubKey
                    //ScriptSig = new Script(p.ScriptPubKey) // could be wrong then take ... prev row
                });
            });

            //tx.AddInput(new TxIn
            //{
            //    PrevOut = new OutPoint(new uint256("953d21861f7d5237563f643df71038d25b9871552eec279dc531d72a3da95362"), 1),
            //    ScriptSig = pubKey.GetAddress(network).ScriptPubKey
            //});

            var destination = BitcoinAddress.Create(toaddress, network);

            // adding output
            tx.AddOutput(new TxOut
            {
                ScriptPubKey = destination.ScriptPubKey,
                Value = Money.Coins(burnMogs)
            });

            // check if we need to add a change output
            if ((unspentAmount - burnMogs) > txFee)
            {
                tx.AddOutput(new TxOut
                {
                    ScriptPubKey = pubKey.GetAddress(network).ScriptPubKey,
                    Value = Money.Coins(unspentAmount - burnMogs - txFee)
                });
            }

            //tx.ToString(RawFormat.);

            _log.Info($"rawTx: {tx.ToHex()}");

            // sign transaction
            tx.Sign(extkey.PrivateKey, false);

            _log.Info($"sigTx: {tx.ToHex()}");

            return tx;
        }
    }
}
