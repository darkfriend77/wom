using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoMInterface.Game.Interaction;
using WoMInterface.Game.Model;

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
                new Shift(0D,
                1530914381,
                "32ad9e02792599dfdb6a9d0bc0b924da23bd96b1b7eb4f0a68",
                7234,
                "00000000090d6c6b058227bb61ca2915a84998703d4444cc2641e6a0da4ba37e",
                2,
                "163d2e383c77765232be1d9ed5e06749a814de49b4c0a8aebf324c0e9e2fd1cf",
                1.00m,
                0.0001m)
            };
            var mogwai = new Mogwai("addr1", shifts);

            Assert.AreEqual(1, mogwai.CurrentLevel);
            mogwai.AddExp(300, shifts[0]);
            Assert.AreEqual(1, mogwai.CurrentLevel);
            mogwai.AddExp(700, shifts[0]);
            Assert.AreEqual(2, mogwai.CurrentLevel);
            mogwai.AddExp(1000, shifts[0]);
            Assert.AreEqual(3, mogwai.CurrentLevel);
            mogwai.AddExp(1000, shifts[0]);
            Assert.AreEqual(4, mogwai.CurrentLevel);
            mogwai.AddExp(1000, shifts[0]);
            Assert.AreEqual(5, mogwai.CurrentLevel);
        }
    }
}
