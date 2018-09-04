using log4net;
using NBitcoin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WoMApiTest.Tool;

namespace WoMApiTest.Model.Node
{
    public class WalletFile
    {
        public string wifKey;

        public byte[] chainCode;

        public uint seed = 0;

        public Dictionary<string, uint> EncryptedSecrets = new Dictionary<string, uint>();

        public WalletFile(string wifKey, byte[] chainCode)
        {
            this.wifKey = wifKey;
            this.chainCode = chainCode;
        }

    }

    public class MogwaiKeys
    {
        private readonly ExtKey extkey;

        private readonly Network network;

        private readonly PubKey pubKey;

        private readonly PubKey mirrorPubKey;

        public string Address => pubKey.GetAddress(network).ToString();

        public string MirrorAddress => mirrorPubKey?.GetAddress(network).ToString();

        public bool HasMirrorAddress => mirrorPubKey != null;

        public MogwaiKeys(ExtKey extkey, Network network)
        {
            this.extkey = extkey;
            this.network = network;
            this.pubKey = extkey.PrivateKey.PubKey;
            Console.WriteLine(pubKey);
            if (TryMirrorPubKey(extkey.PrivateKey.PubKey, out PubKey mirrorPubKey))
            {
                Console.WriteLine(mirrorPubKey);
                this.mirrorPubKey = mirrorPubKey;
            }
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
            catch (Exception ex)
            {
                //Console.WriteLine(ex.InnerException);
                mirrorPubKey = new PubKey(mirrorPubKeyBytes, true);
                return true;
            }
        }

    }

    public class MogwaiWallet
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Network network = NBitcoin.Altcoins.Mogwai.Instance.Mainnet;

        private readonly string path;

        private WalletFile walletFile;

        private ExtKey extKey;

        private Mnemonic mnemo;
        public string MnemonicWords
        {
            get
            {
                string mnemoStr = mnemo != null ? mnemo.ToString() : string.Empty;
                mnemo = null;
                return mnemoStr;
            }
        }

        public MogwaiWallet(string password, string path)
        {
            this.path = path;

            if (!TryReadFile(path, out walletFile))
            {
                mnemo = new Mnemonic(Wordlist.English, WordCount.Twelve);
                extKey = mnemo.DeriveExtKey(password);
                //extKey = new ExtKey();
                var chainCode = extKey.ChainCode;
                var encSecretWif = extKey.PrivateKey.GetEncryptedBitcoinSecret(password, network).ToWif();
                walletFile = new WalletFile(encSecretWif, chainCode);
                Persist(path, walletFile);
            }
            else
            {
                var masterKey = Key.Parse(walletFile.wifKey, password, network);
                extKey = new ExtKey(masterKey, walletFile.chainCode);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="persist"></param>
        /// <returns></returns>
        public MogwaiKeys GetMogwaiKeys(uint seed, bool persist = false)
        {
            var extKeyDerived = extKey.Derive(seed);
            var wif = extKey.PrivateKey.GetWif(network);
            if (persist)
            {
                var encSecretWif = extKey.PrivateKey.GetEncryptedBitcoinSecret(extKey.ToString(network), network).ToWif();
                if (!walletFile.EncryptedSecrets.ContainsKey(encSecretWif))
                {
                    walletFile.EncryptedSecrets[encSecretWif] = seed;
                }
                Persist(path, walletFile);
            }
            return new MogwaiKeys(extKeyDerived, network);
        }

        public void Write()
        {
            Persist(path, walletFile);
        }

        private bool TryReadFile<T>(string path, out T obj)
        {
            obj = default(T);

            if (!File.Exists(path))
            {
                return false;
            }

            try
            {
                var objDecrypted = Decrypt(File.ReadAllText(path));
                obj = JsonConvert.DeserializeObject<T>(objDecrypted);
                return true;
            }
            catch (Exception e)
            {
                _log.Error($"TryDBFile[{obj.GetType()}]: {e}");
                return false;
            }
        }

        private void IsValidPubKey()
        {
            //TODO
            //Cryptography.ECDSA.Secp256K1Manager.IsCanonical()
        }

        private void Persist<T>(string path, T obj)
        {
            string objEncrypted = Encrypt(JsonConvert.SerializeObject(obj));
            File.WriteAllText(path, objEncrypted);
        }

        private string Encrypt(string str)
        {
            return str;
        }

        private string Decrypt(string str)
        {
            return str;
        }
    }
}
