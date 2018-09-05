using NBitcoin;
using System;
using WoMApi.Tool;

namespace WoMApi.Model.Node
{
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
            catch (Exception ex)
            {
                //Console.WriteLine(ex.InnerException);
                mirrorPubKey = new PubKey(mirrorPubKeyBytes, true);
                return true;
            }
        }

    }
}
