using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WoMInterface.Game.Model;

namespace WoMInterface.Tool
{
    public class CachingService
    {
        public class MogwaisDB
        {
            public Dictionary<string, int> MogwaiPointers = new Dictionary<string, int>();

            public void Update(Mogwai mogwai)
            {
                MogwaiPointers[mogwai.Key] = mogwai.Pointer;
            }

            public int Pointer(Mogwai mogwai)
            {
                if (MogwaiPointers.ContainsKey(mogwai.Key))
                {
                    return MogwaiPointers[mogwai.Key];
                }
                else
                {
                    return 0;
                }
            }
        }

        public class BlockHashDB
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

        private const string pathBlockHashDBFile = "blockhash.db";

        private const string pathMogwaisDBFile = "mogwais.db";

        private BlockHashDB _blockHashDB;
        public BlockHashDB BlockHashCache { get { return _blockHashDB; } set { _blockHashDB = value; } }

        private MogwaisDB _mogwaisDB;
        public MogwaisDB MogwaisCache { get { return _mogwaisDB; } set { _mogwaisDB = value; } }

        public CachingService()
        {
            if (!TryReadDBFile<BlockHashDB>(pathBlockHashDBFile, out _blockHashDB))
            {
                _blockHashDB = new BlockHashDB();
            }

            if (!TryReadDBFile<MogwaisDB>(pathMogwaisDBFile, out _mogwaisDB))
            {
                _mogwaisDB = new MogwaisDB();
            }

        }
        public void EmptyCache()
        {

        }

        public bool TryReadDBFile<T>(string pathDBFile, out T dBObj)
        {
            dBObj = default(T);

            if (!File.Exists(pathDBFile))
            {
                return false;
            }

            try
            {
                var dBstr = File.ReadAllText(pathDBFile);
                dBObj = JsonConvert.DeserializeObject<T>(dBstr);
                return true;
            }
            catch (Exception e)
            {
                _log.Error($"TryDBFile[{dBObj.GetType()}]: {e}");
                return false;
            }
        }

        //public bool TryReadBlockHashDBFile(out BlockHashDB blockHashDBObj)
        //{
        //    if (!File.Exists(pathBlockHashDBFile))
        //    {
        //        blockHashDBObj = null;
        //        return false;
        //    }

        //    try
        //    {
        //        var blockHashDBstr = File.ReadAllText(pathBlockHashDBFile);
        //        blockHashDBObj = JsonConvert.DeserializeObject<BlockHashDB>(blockHashDBstr);
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        _log.Error($"TryReadBlockHashDBFile: {e}");
        //        blockHashDBObj = null;
        //        return false;
        //    }
        //}

        public void Persist(bool blockhashdb, bool mogwaisdb)
        {
            if (blockhashdb)
            {
                string blockHashDBstr = JsonConvert.SerializeObject(_blockHashDB);
                File.WriteAllText(pathBlockHashDBFile, blockHashDBstr);
            }

            if (mogwaisdb)
            {
                string mogwaisDBstr = JsonConvert.SerializeObject(_mogwaisDB);
                File.WriteAllText(pathMogwaisDBFile, mogwaisDBstr);
            }
        }
    }
}