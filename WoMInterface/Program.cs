using BitcoinLib.Services.Coins.Cryptocoin;
using BitcoinLib.Services.Coins.Mogwaicoin;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game;
using WoMInterface.Node;
using WoMInterface.Tool;

namespace WoMInterface
{
    class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {           

            XmlConfigurator.Configure();

            CommandLine.Instance.Start();

            //_log.Info($"WalletVersion: {Blockchain.Instance.GetInfo().WalletVersion}");

            //Blockchain.Instance.CreateMogwai("MMwQDLyormYZzKjiMpqEJguYnmJ3ZYofUV");

            //Blockchain.Instance.TryGetMogwai("MMwQDLyormYZzKjiMpqEJguYnmJ3ZYofUV", out Mogwai mogwai);
            //mogwai.Print();

            //Console.WriteLine(string.Format("{0:P4}", Test.GetProbability("1234", 62, 10000)));
            //GHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz
            //string x = "M1wQDLyormYZzKjiMpqEJguYnmJ3ZYofUV";
            //var encode = Base58Encoding.ByteArrayToString(Base58Encoding.Decode("MMwQDLyormYZzKjiMpqEJguYnmJ3ZYofUV"));
            //Console.WriteLine(encode.Length);
            //Console.WriteLine(encode);
            //Console.WriteLine(Base58Encoding.Encode(Base58Encoding.StringToByteArray(encode)));
            //Console.WriteLine(x.Length);
            //for (int i = 0; i < 100;i++)
            //{
            //    Console.WriteLine(Base58Encoding.Encode(Base58Encoding.StringToByteArray("34" + Test.GetRandomHexNumber(48))));
            //}


            //List<Shift> shiftListTest = new List<Shift>() {
            //    new Shift() {
            //        Time = 1531985792,
            //        AdHex = HexUtil.ByteArrayToString(Base58Encoding.Decode("MDAqJYR47seXxqNze3PdDPaBuCu3hkezuz")),
            //        Height = 15241,
            //        BkHex = "0000000367770e3405fe359292e61c71f11dc8dc229584b5fd1ea006b9b93f3e",
            //        BkIndex = 1,
            //        TxHex = "2673b470893482578caa1449fbb1512152c212238ab847f856b4ac3481dd8737",
            //        Amount = 1.08877788M
            //    }
            //};
            //Mogwai mogwaitest = new Mogwai(shiftListTest);
            //mogwaitest.Print();
            //Console.ReadKey();

        }


    }
}
