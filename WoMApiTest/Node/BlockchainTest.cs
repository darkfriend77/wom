using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NBitcoin;

namespace WoMApi.Node
{
    [TestClass]
    public class BlockchainTest
    {
        [TestMethod]
        public void GetBlock()
        {
            var blockResponse = Blockchain.Instance.GetBlock("00000000077d796dabe050ee7d80c4e329b601e263c3e522d3abf2fbf9a9263f");
            Assert.AreEqual("00000000077d796dabe050ee7d80c4e329b601e263c3e522d3abf2fbf9a9263f", blockResponse.Hash);
        }

        [TestMethod]
        public void GetBlockHash()
        {
            var blockResponse = Blockchain.Instance.GetBlockHash(47863);
            Assert.AreEqual("00000000077d796dabe050ee7d80c4e329b601e263c3e522d3abf2fbf9a9263f", blockResponse);
        }

        [TestMethod]
        public void GetBlockCount()
        {
            var blockResponse = Blockchain.Instance.GetBlockCount();
            Assert.IsTrue(blockResponse > 40000);

        }

        [TestMethod]
        public void GetBalance()
        {
            var blockResponse1 = Blockchain.Instance.GetBalance("MFTHxujEGC7AHNBMCWQCuXZgVurWjLKc5e");
            Assert.AreEqual(8.98938803m, blockResponse1);

            var blockResponse2 = Blockchain.Instance.GetBalance("MWG1HtzRAjZMxQDzeoFoHQbzDygGR13aWG");
            Assert.AreEqual(8.99999m, blockResponse2);
        }

        [TestMethod]
        public void GetUnspent()
        {
            var blockResponse0 = Blockchain.Instance.GetUnspent(0, 999999, "MDvzt3q937NpCvawXVttFXueSDBson3bWH");
            Assert.AreEqual(150, blockResponse0.Count);
            Assert.AreEqual("MDvzt3q937NpCvawXVttFXueSDBson3bWH", blockResponse0[0].Address);
            Assert.AreEqual(1000m, blockResponse0[0].Amount);
            Assert.AreEqual("498a37b770eafce6e112e5c2d9ac6ded2beedcd2965b489900f1069de04942fa", blockResponse0[0].Txid);

            var blockResponse1 = Blockchain.Instance.GetUnspent(0, 999999, "MCmpMFvQXeGQxJSSdCuPEf58v5iePJesN5");
            Assert.AreEqual(1, blockResponse1.Count);

            var blockResponse2 = Blockchain.Instance.GetUnspent(0, 999999, "MWG1HtzRAjZMxQDzeoFoHQbzDygGR13aWG");
            Assert.AreEqual(1, blockResponse2.Count);
        }

        [TestMethod]
        public void ListTransaction()
        {
            var blockResponse = Blockchain.Instance.ListTransaction("MJHYMxu2kyR1Bi4pYwktbeCM7yjZyVxt2i");
            Assert.AreEqual(3, blockResponse.Count);
            Assert.AreEqual("MJHYMxu2kyR1Bi4pYwktbeCM7yjZyVxt2i", blockResponse[0].Address);
            Assert.AreEqual("00000000759514788f612f69606edd5751d517c39880f72b03772e26827671c4", blockResponse[0].Blockhash);
            Assert.AreEqual("receive", blockResponse[0].Category);
        }

        [TestMethod]
        public void BurnMogs()
        {
            MogwaiWallet wallet = new MogwaiWallet("1234", "test.dat");

            var mogwaiKeys0 = wallet.MogwaiKeyDict["MWG1HtzRAjZMxQDzeoFoHQbzDygGR13aWG"];
            Assert.AreEqual("MWG1HtzRAjZMxQDzeoFoHQbzDygGR13aWG", mogwaiKeys0.Address);

            var mogwaiKeys1 = wallet.MogwaiKeyDict["MNtnWbBjUhRvNnd9YxM2mnxeLPNkxb4Fio"];
            Assert.AreEqual("MNtnWbBjUhRvNnd9YxM2mnxeLPNkxb4Fio", mogwaiKeys1.Address);

            var blockResponse0 = Blockchain.Instance.GetBalance("MWG1HtzRAjZMxQDzeoFoHQbzDygGR13aWG");
            Assert.AreEqual(8.99999m, blockResponse0);

            var blockResponse1 = Blockchain.Instance.GetBalance("MNtnWbBjUhRvNnd9YxM2mnxeLPNkxb4Fio");
            Assert.AreEqual(12.12344678m, blockResponse1);

            var unspentTxList = Blockchain.Instance.GetUnspent(6, 9999999, mogwaiKeys0.Address);
            var unspentAmount = unspentTxList.Sum(p => p.Amount);

            // create transaction
            Transaction tx = mogwaiKeys0.CreateTransaction(unspentTxList, unspentAmount, mogwaiKeys1.Address, 1.0m, 0.00001m);
            Assert.AreEqual("01000008000118ddf505000000001976a914a477c1319360114de9f3ed88381cc4dfa9147f3288ac00000000", tx.ToHex());

            var blockResponse = Blockchain.Instance.SendRawTransaction(tx.ToHex());
            Assert.AreEqual("", blockResponse);
        }

        [TestMethod]
        public void GetShifts()
        {
            //var shifts = Blockchain.Instance.GetShifts("MWG1HtzRAjZMxQDzeoFoHQbzDygGR13aWG", "MNtnWbBjUhRvNnd9YxM2mnxeLPNkxb4Fio", out bool openShifts);
            //Assert.AreEqual(3, shifts.Count);

        }



    }
}
