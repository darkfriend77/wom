using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WoMInterface.Tool
{
    internal class CachingService
    {
        public class CacheDB
        {
            public Dictionary<int, string> BlockHashDict = new Dictionary<int, string>();

            internal int MaxBlockHash { get {
                    var list = BlockHashDict.Keys.ToList();
                    if (list.Count == 0)
                        return 0;
                    return list.Max();
                } }
        }

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string pathCacheDBFile = "cache.db";

        private CacheDB _cacheDB;
        public CacheDB Cache { get { return _cacheDB; } set { _cacheDB = value; } }

        public CachingService()
        {
            if (!TryReadCacheDBFile(out _cacheDB))
            {
                _cacheDB = new CacheDB();
            }
        }
        public void EmptyCache()
        {

        }

        public bool TryReadCacheDBFile(out CacheDB cacheDBObj)
        {
            if (!File.Exists(pathCacheDBFile))
            {
                cacheDBObj = null;
                return false;
            }

            try
            {
                var cacheDBstr = File.ReadAllText(pathCacheDBFile);
                cacheDBObj = JsonConvert.DeserializeObject<CacheDB>(cacheDBstr);
                return true;
            }
            catch (Exception e)
            {
                _log.Error($"TryReadCacheDBFile: {e}");
                cacheDBObj = null;
                return false;
            }
        }

        public void Persist()
        {
            string cacheDBstr = JsonConvert.SerializeObject(_cacheDB);
            File.WriteAllText(pathCacheDBFile, cacheDBstr);
        }
    }
}