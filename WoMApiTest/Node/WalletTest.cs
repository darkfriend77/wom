using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WoMApi.Node
{
    [TestClass]
    public class Wallet
    {
        [TestMethod]
        public void TestMethod1()
        {
            MogwaiWallet wallet = new MogwaiWallet("1234", "test.dat");
            var mogwaiKeys = wallet.MogwaiKeyDict["MWG1HtzRAjZMxQDzeoFoHQbzDygGR13aWG"];
            Assert.AreEqual("MWG1HtzRAjZMxQDzeoFoHQbzDygGR13aWG", mogwaiKeys.Address);
            Assert.AreEqual(true, mogwaiKeys.HasMirrorAddress);
            Assert.AreEqual("MLTNLAojhmBHF3BMzG3RmzoQ1bnbnxxdeD", mogwaiKeys.MirrorAddress);
        }
    }
}
