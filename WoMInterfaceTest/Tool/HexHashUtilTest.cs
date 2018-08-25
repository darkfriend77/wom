using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoMInterface.Game;
using WoMInterface.Game.Interaction;

namespace WoMInterface.Tool.Tests
{
    [TestClass]
    public class HexHashUtilTest
    {
        [TestMethod]
        public void HashSHA256Test()
        {
            Shift shift = new Shift(0D)
            {
                Time = 1530914381,
                BkIndex = 2,
                Amount = 1,
                Height = 7234,
                AdHex = "32ad9e02792599dfdb6a9d0bc0b924da23bd96b1b7eb4f0a68",
                BkHex = "00000000090d6c6b058227bb61ca2915a84998703d4444cc2641e6a0da4ba37e",
                TxHex = "163d2e383c77765232be1d9ed5e06749a814de49b4c0a8aebf324c0e9e2fd1cf"
            };

            var bytes = HexHashUtil.StringToByteArray(shift.AdHex);
            var hashedBytes = HexHashUtil.HashSHA256(bytes);
            var hash = HexHashUtil.ByteArrayToString(hashedBytes);
            Assert.AreEqual("bcd35d9f6de167fada34bfeb0c59d2b1a55974b9460c54f42dfff2e86cf8c58b", hash);
        }
    }
}
