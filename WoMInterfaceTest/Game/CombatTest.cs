using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoMInterface.Game.Interaction;

namespace WoMInterface.Game.Tests
{
    [TestClass]
    public class CombatTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Shift shift = new Shift(0D)
            {
                Time = 1531171420,
                AdHex = "32f13027e869de56de3c2d5af13f572b67b5e75a18594013ec",
                Height = 9196,
                BkHex = "000000001f2ade78b094fce0fbfacc55da3a23ec82489171eb2687a1b6582d12",
                BkIndex = 11,
                TxHex = "9679a3d39efdf8faa019410250fa91647a76cbb1bd2fd1c5d7ba80551b4edd7b",
                Amount = 1
            };

        }
    }
}
