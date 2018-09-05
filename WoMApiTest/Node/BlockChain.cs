using log4net;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WoMApiTest.Model.Node;

namespace WoMApiTest.Node
{
    public class Blockchain
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static Blockchain instance;

        public static Blockchain Instance => instance ?? (instance = new Blockchain());

        private RestClient client;

        private Blockchain()
        {
            client = new RestClient(ConfigurationManager.AppSettings["apiUrl"]);

        }

        public IRestResponse<Block> GetBlock(int value)
        {
            var request = new RestRequest("getblock/{id}", Method.GET);
            request.AddUrlSegment("id", value.ToString());
            IRestResponse<Block> blockResponse = client.Execute<Block>(request);
            return blockResponse;
        }

        public decimal GetBalance(string address)
        {
            var request = new RestRequest("getbalance/{address}", Method.GET);
            request.AddUrlSegment("address", address);
            IRestResponse<decimal> blockResponse = client.Execute<decimal>(request);
            return blockResponse.Data;
        }

    }
}
