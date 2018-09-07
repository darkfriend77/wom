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
using WoMApi.Tool;

namespace WoMApi.Node
{
    public class WalletFile
    {
        public string wifKey;

        public byte[] chainCode;

        public Dictionary<string, uint> EncryptedSecrets = new Dictionary<string, uint>();

        public WalletFile(string wifKey, byte[] chainCode)
        {
            this.wifKey = wifKey;
            this.chainCode = chainCode;
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

        public Dictionary<string, MogwaiKeys> MogwaiKeyDict { get; set; } = new Dictionary<string, MogwaiKeys>();

        public bool IsUnlocked => extKey != null;

        public bool IsCreated => walletFile != null;

        /// <summary>
        /// 
        /// </summary>
        public MogwaiWallet()
        {
            this.path = ConfigurationManager.AppSettings["walletFile"];

            if (!Caching.TryReadFile(path, out walletFile))
            {
            } 
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="password"></param>
        /// <param name="path"></param>
        public MogwaiWallet(string password, string path)
        {
            this.path = path;

            if (!Caching.TryReadFile(path, out walletFile))
            {
                Create(password);
            }
            else
            {
                Unlock(password);
            }
        }

        /// <summary>
        /// Create a new wallet with mnemoic
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Create(string password)
        {
            if (IsCreated)
            {
                return true;
            }

            mnemo = new Mnemonic(Wordlist.English, WordCount.Twelve);
            extKey = mnemo.DeriveExtKey(password);
            var chainCode = extKey.ChainCode;
            var encSecretWif = extKey.PrivateKey.GetEncryptedBitcoinSecret(password, network).ToWif();
            walletFile = new WalletFile(encSecretWif, chainCode);
            Caching.Persist(path, walletFile);
            return true;
        }

        /// <summary>
        /// Unlock a locked wallet.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Unlock(string password)
        {
            if (IsUnlocked || !IsCreated)
            {
                return IsUnlocked && IsCreated;
            }

            var masterKey = Key.Parse(walletFile.wifKey, password, network);
            extKey = new ExtKey(masterKey, walletFile.chainCode);
            
            // finally load all mogwaikeys
            LoadMogwaiKeys();

            return true;
        }

        private void LoadMogwaiKeys()
        {
            foreach ( var seed in walletFile.EncryptedSecrets.Values)
            {
                var mogwaiKey = GetMogwaiKeys(seed);
                if (!MogwaiKeyDict.ContainsKey(mogwaiKey.Address))
                {
                    MogwaiKeyDict[mogwaiKey.Address] = mogwaiKey;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mogwaiKeys"></param>
        /// <param name="tryes"></param>
        /// <returns></returns>
        public bool GetNewMogwaiKey(out MogwaiKeys mogwaiKeys, int tryes = 10)
        {
            mogwaiKeys = null;

            if (!IsUnlocked)
            {
                return false; ;
            }

            uint seed = 1000;
            if (walletFile.EncryptedSecrets.Values.Count > 0)
            {
                seed = walletFile.EncryptedSecrets.Values.Max() + 1;
            }

            for (uint i = seed; i < seed + tryes; i++)
            {
                var mogwayKeysTemp = GetMogwaiKeys(i);
                if (mogwayKeysTemp.HasMirrorAddress)
                {
                    var wif = mogwayKeysTemp.GetEncryptedSecretWif();
                    if (!walletFile.EncryptedSecrets.ContainsKey(wif))
                    {
                        walletFile.EncryptedSecrets[wif] = i;
                        mogwaiKeys = mogwayKeysTemp;
                        Caching.Persist(path, walletFile);
                        return true;
                    }
 
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="persist"></param>
        /// <returns></returns>
        private MogwaiKeys GetMogwaiKeys(uint seed)
        {
            var extKeyDerived = extKey.Derive(seed);
            var wif = extKey.PrivateKey.GetWif(network);
            return new MogwaiKeys(extKeyDerived, network);
        }
    }

}
