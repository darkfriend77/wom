using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoMInterface.Game.Interaction;
using WoMInterface.Game.Model;

namespace WoMInterfaceTest.Game
{
    [TestClass]
    public class DungeonTest
    {
        [TestMethod]
        public void SimpleRoomTest()
        {
            Shift shift = new Shift(0D,
                1531171420,
                "32f13027e869de56de3c2d5af13f572b67b5e75a18594013ec",
                9196,
                "000000001f2ade78b094fce0fbfacc55da3a23ec82489171eb2687a1b6582d13",
                11,
                "9679a3d39efdf8faa019410250fa91647a76cbb1bd2fd1c5d7ba80551b4edd7b",
                1.00m,
                0.0001m);

            
            
        }

        [TestMethod]
        public void RoomTileTest()
        {
            var room = new SimpleRoom(null, null);
            var path = room._mog.CurrentTile.GetShortestPath(room._mobs[0].CurrentTile);
        }
    }
}
