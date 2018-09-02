using RestSharp;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMApiTest.Node;

namespace WoMApiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 100; i++)
            {
                var blockResponse = BlockChain.Instance.GetBlock(i);
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
