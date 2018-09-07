using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoMInterface.Game.Enums;
using WoMInterface.Game.Interaction;
using WoMInterface.Game.Model;
using WoMInterface.Node;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WoMInterface.Tool;

namespace WoMInterfaceTest.Game
{
    [TestClass]
    public class DungeonTest
    {
        private static readonly Mogwai TestMog;
        private static readonly Dictionary<double, Shift> TestShifts;

        static DungeonTest()
        {
            var json = Encoding.UTF8.GetString(Properties.Resources.mogwai);

            TestShifts = JsonConvert.DeserializeObject<Dictionary<double, Shift>>(json);
            TestMog = new Mogwai("MJHYMxu2kyR1Bi4pYwktbeCM7yjZyVxt2i", TestShifts);
        }

        [TestMethod]
        public void SimpleDungeonTest()
        {
            var mogwai = TestMog;
            mogwai.EnterSimpleDungeon();
        }
    }
}
