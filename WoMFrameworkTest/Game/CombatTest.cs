﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoMFramework.Game.Interaction;

namespace WoMFramework.Game.Tests
{
    [TestClass]
    public class CombatTest
    {
        [TestMethod]
        public void TestMethod1()
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
    }
}