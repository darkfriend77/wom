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

        private MogwaiWallet wallet;

        private static Blockchain instance;

        public static Blockchain Instance => instance ?? (instance = new Blockchain());

        private RestClient client;

        private Blockchain()
        {
            client = new RestClient(ConfigurationManager.AppSettings["apiUrl"]);

        }

        public void Unlock(string password, string walletFile = null)
        {
            string path = walletFile ?? ConfigurationManager.AppSettings["walletFile"];

            wallet = new MogwaiWallet(password, path);
        }

        public IRestResponse<Block> GetBlock(int value)
        {
            var request = new RestRequest("getblock/{id}", Method.GET);
            request.AddUrlSegment("id", value.ToString());
            IRestResponse<Block> blockResponse = client.Execute<Block>(request);
            return blockResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mogwaiAddress"></param>
        /// <param name="tryes"></param>
        /// <returns></returns>
        public bool CreateMogwaiAddress(out string mogwaiAddress, int tryes = 10)
        {
            mogwaiAddress = string.Empty;

            //wallet.Secret.PubKey.Derivate();

            return true;
        }
    }
}
