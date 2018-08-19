using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoMInterface.Game;
using WoMInterface.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMInterface.Game.Tests
{
    [TestClass()]
    public class MogwaiTest
    {

        [TestMethod()]
        public void TryHexPosConversionTest()
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
            Assert.IsTrue(HexUtil.TryHexPosConversion(1, 2, new List<char[]>() { shifts[0].BkHex.ToCharArray(), shifts[0].TxHex.ToCharArray() }, out double coat));
            Assert.AreEqual(99, coat);
        }

        [TestMethod()]
        public void GetExpPatternsTest()
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
            var mogwai = new Mogwai("addr1",shifts);

            var stringArray = mogwai.Experience.GetExpPatterns(shifts[0].TxHex);
            Assert.AreEqual(18, stringArray.Length);
            Assert.AreEqual("163d", stringArray[0]);
            Assert.AreEqual("2e38", stringArray[1]);
            Assert.AreEqual("3c77", stringArray[2]);
            Assert.AreEqual("7652", stringArray[3]);
            Assert.AreEqual("32be", stringArray[4]);

            Assert.AreEqual("cf", stringArray[17]);
        }
    }
}