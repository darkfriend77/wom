using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMApiTest.Node
{
    public class BlockChain
    {
        private static BlockChain instance;
        public static BlockChain Instance => instance ?? (instance = new BlockChain());

        private RestClient client;

        private BlockChain()
        {
            client = new RestClient("https://cristof.crabdance.com/mogwai/");
        }

        public IRestResponse<Block> GetBlock(int value)
        {
            var request = new RestRequest("getblock/{id}", Method.GET);
            request.AddUrlSegment("id", value.ToString());
            IRestResponse<Block> blockResponse = client.Execute<Block>(request);
            return blockResponse;
        }
    }
}
