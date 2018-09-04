using RestSharp;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMApiTest.Node;
using NBitcoin;
using WoMApiTest.Model.Node;
using WoMApiTest.Tool;

namespace WoMApiTest
{
    class Program
    {
        static void Main(string[] args)
        {

            //  "isvalid": true,
            //  "address": "MQ6JnKWAiDkN2eo6c19647RBRzUecryRdP",
            //  "scriptPubKey": "76a914b19db9a389dc6f62c2a1809ea3cdcd4c3a91c68988ac",
            //  "ismine": true,
            //  "iswatchonly": false,
            //  "isscript": false,
            //  "pubkey": "021cb06967a8ac6972165eb5535acabd91e5c1a72a679d99983e9a3ab3b90bd378",
            //  "iscompressed": true,
            //  "mirkey": "021cb069673db09b3ba3a9e38999d976a27a1c5e19dbaca5355be5612796ca8a78",
            //  "mirkeyvalid": true,
            //  "ismiraddrvalid": true,
            //  "miraddress": "MQN7moivGfWPiwUfCX1PzfqqYN29gKsgEb"

            MogwaiWallet wallet = new MogwaiWallet("1234", "eeee.dat");
            Console.WriteLine(wallet.MnemonicWords);
            var mogwaiKeys = wallet.GetMogwaiKeys(1);
            Console.WriteLine(mogwaiKeys.Address);
            Console.WriteLine(mogwaiKeys.MirrorAddress);
            Console.ReadKey();

        }

        public static void TestPrivateKey()
        {
            Network network = NBitcoin.Altcoins.Mogwai.Instance.Mainnet;
            var key = new ExtKey();
            var chainCode = key.ChainCode;
            //Console.WriteLine(HexUtil.ByteArrayToString(chainCode));
            var secret = key.PrivateKey.GetBitcoinSecret(network);
            var encSecret = key.PrivateKey.GetEncryptedBitcoinSecret("1234", network);
            var encSecretWif = encSecret.ToWif();
            Console.WriteLine($"Wif(MasterKey): {encSecretWif}");

            var key1 = key.Derive(1);
            Console.WriteLine($"{key1.PrivateKey.PubKey.GetAddress(network)}");


            string str = Console.ReadLine();

            var rPrivateKey = Key.Parse(encSecretWif, str, network);
            var rKey = new ExtKey(rPrivateKey, chainCode);
            var rKey1 = rKey.Derive(1);
            Console.WriteLine($"{rKey1.PrivateKey.PubKey.GetAddress(network)}");

        }

        public static void TestSecret()
        {
            Network network = NBitcoin.Altcoins.Mogwai.Instance.Mainnet;
            var key = new Key();
            var secret = key.GetBitcoinSecret(network);
            var encSecret = key.GetEncryptedBitcoinSecret("1234", network);
            var encSecretWif = encSecret.ToWif();
            Console.WriteLine($"Wif: {encSecretWif}");
            Console.WriteLine(key.PubKey.GetAddress(network));
            string str = Console.ReadLine();

            var rKey = Key.Parse(encSecretWif, str, network);
            Console.WriteLine(rKey.PubKey.GetAddress(network));
            Console.ReadKey();
        }

        public static void TestApi()
        {


            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 100; i++)
            {
                var blockResponse = Blockchain.Instance.GetBlock(i);
                if (blockResponse.Data != null)
                {
                    Console.WriteLine($"{i}: {blockResponse.Data.Hash}");
                }
                else
                {
                    Console.WriteLine($"{blockResponse.Content}");
                }
            }
            sw.Stop();

            Console.WriteLine("Elapsed={0}", sw.Elapsed);
            Console.ReadKey();

        }
    }
}
