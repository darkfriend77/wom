using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoMInterface.Game;

namespace WoMInterface.Tool.Tests
{
    [TestClass]
    public class NameGenTest
    {
        [TestMethod]
        public void GenerateNameTest()
        {
            HexValue hexValue = new HexValue(
            new Shift()
            {
                Time = 1530914381,
                BkIndex = 2,
                Amount = 1,
                Height = 7234,
                AdHex = "32ad9e02792599dfdb6a9d0bc0b924da23bd96b1b7eb4f0a68",
                BkHex = "00000000090d6c6b058227bb61ca2915a84998703d4444cc2641e6a0da4ba37e",
                TxHex = "163d2e383c77765232be1d9ed5e06749a814de49b4c0a8aebf324c0e9e2fd1cf"
            });
            Assert.AreEqual("Vihiryne", NameGen.GenerateName(hexValue));
        }
    }
}
