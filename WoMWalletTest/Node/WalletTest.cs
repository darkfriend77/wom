﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WoMWallet.Node
{
    [TestClass]
    public class WalletTest
    {
        [TestMethod]
        public void WalletTestPersist()
        {
            MogwaiWallet wallet = new MogwaiWallet("1234", "test.dat");
            var mogwaiKeys = wallet.MogwaiKeyDict["MWG1HtzRAjZMxQDzeoFoHQbzDygGR13aWG"];
            Assert.AreEqual("MWG1HtzRAjZMxQDzeoFoHQbzDygGR13aWG", mogwaiKeys.Address);
            Assert.AreEqual(true, mogwaiKeys.HasMirrorAddress);
            Assert.AreEqual("MLTNLAojhmBHF3BMzG3RmzoQ1bnbnxxdeD", mogwaiKeys.MirrorAddress);
        }

        [TestMethod]
        public void WalletCreation()
        {
            MogwaiWallet wallet = new MogwaiWallet();
            Assert.IsFalse(wallet.IsCreated);
            Assert.IsFalse(wallet.IsUnlocked);
        }

        [TestMethod]
        public void WalletUnlock()
        {
            MogwaiWallet wallet = new MogwaiWallet("test.dat");
            Assert.IsTrue(wallet.IsCreated);
            Assert.IsFalse(wallet.IsUnlocked);
            wallet.Unlock("1234");
            Assert.IsTrue(wallet.IsUnlocked);
        }

        [TestMethod]
        public void WalletDeposit()
        {
            MogwaiWallet wallet = new MogwaiWallet("test.dat");
            Assert.IsNull(wallet.Deposit);
            wallet.Unlock("1234");
            Assert.IsNotNull(wallet.Deposit);
            Assert.AreEqual("MBAdzUJU1zyUJLfiUDuvU8zWjenxzi7ZF6", wallet.Deposit.Address);
        }
    }
}
