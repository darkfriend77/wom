using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WoMInterface.Game.Tests
{
    [TestClass]
    public class ExperienceTest
    {
        [TestMethod]
        public void GetLevel()
        {
            List<Shift> shifts = new List<Shift>()
            {
                new Shift()
                {
                       Time = 1530914381,
                       BkIndex = 2,
                       Amount = 1,
                       Height = 7234,
                       AdHex = "32ad9e02792599dfdb6a9d0bc0b924da23bd96b1b7eb4f0a68",
                       BkHex = "00000000090d6c6b058227bb61ca2915a84998703d4444cc2641e6a0da4ba37e",
                       TxHex = "163d2e383c77765232be1d9ed5e06749a814de49b4c0a8aebf324c0e9e2fd1cf"
                }
            };
            var mogwai = new Mogwai("addr1", shifts);

            Assert.AreEqual(1, mogwai.CurrentLevel);
            mogwai.AddExp(300, new Shift());
            Assert.AreEqual(1, mogwai.CurrentLevel);
            mogwai.AddExp(700, new Shift());
            Assert.AreEqual(2, mogwai.CurrentLevel);
            mogwai.AddExp(1000, new Shift());
            Assert.AreEqual(3, mogwai.CurrentLevel);
            mogwai.AddExp(1000, new Shift());
            Assert.AreEqual(4, mogwai.CurrentLevel);
            mogwai.AddExp(1000, new Shift());
            Assert.AreEqual(5, mogwai.CurrentLevel);
        }
    }
}
