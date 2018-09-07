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
        public void GetBlockHashes()
        {
            var blockResponse = Blockchain.Instance.GetBlockHashes(0, 100);
            Assert.AreEqual(100, blockResponse.Count());
            Assert.AreEqual("1", blockResponse[1].Block);
            Assert.AreEqual("000004a3418bf6f7a085b0a489d56eea4fbc094be8ec48ad7ec11621a4dd7431", blockResponse[1].Hash);
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
            Assert.AreEqual(2, blockResponse1.Count);

            var blockResponse2 = Blockchain.Instance.GetUnspent(0, 999999, "MWG1HtzRAjZMxQDzeoFoHQbzDygGR13aWG");
            Assert.AreEqual(1, blockResponse2.Count);
        }

        [TestMethod]
        public void ListTransaction()
        {
            var blockResponse = Blockchain.Instance.ListTransactions("MJHYMxu2kyR1Bi4pYwktbeCM7yjZyVxt2i");
            Assert.AreEqual(3, blockResponse.Count);
            Assert.AreEqual("MJHYMxu2kyR1Bi4pYwktbeCM7yjZyVxt2i", blockResponse[0].Address);
            Assert.AreEqual("00000000759514788f612f69606edd5751d517c39880f72b03772e26827671c4", blockResponse[0].Blockhash);
            Assert.AreEqual("receive", blockResponse[0].Category);
        }

        [TestMethod]
        public void ListMirrorTransaction()
        {
            var blockResponse = Blockchain.Instance.ListMirrorTransactions("MEYUySQDPzgbTuZSjGfPVikgHtDJZHL8WE");
            Assert.AreEqual(4, blockResponse.Count);
            Assert.AreEqual("41317", blockResponse[0].Height);
            Assert.AreEqual("000000003f6f8fa173c4d2ddaa07911b06fb41a72744dc646de29377fc04b19e", blockResponse[0].Blockhash);
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
            Assert.AreEqual("01000000019d0262999e5eacc32c2c6921e730d57a7e51938c2d3e22158979c72a7be318e3010000006a4730440220444d777a2e5c4c72941303e7bf7c5734a7ba69087b5b10b370bd685a064c97f402205b8d476d9c73ada7584ce851df9fc675bfd8d4bf8439ee94e52e7f81856885b5012103007f99a5c4754d67c9fed1852ed451bec7371c1b0907b8488ee5aa6593b865c4ffffffff0218ddf505000000001976a914a477c1319360114de9f3ed88381cc4dfa9147f3288ac3000af2f000000001976a914f5440a1dd1ada4c5b4160b8c754f9148eb4a505388ac00000000", tx.ToHex());

            //var blockResponse = Blockchain.Instance.SendRawTransaction(tx.ToHex());
            //Assert.AreEqual("", blockResponse);
        }

        [TestMethod]
        public void GetShifts()
        {
            var shifts = Blockchain.Instance.GetShifts("MEYUySQDPzgbTuZSjGfPVikgHtDJZHL8WE");
            Assert.IsTrue(shifts.Count > 6734);
            Assert.AreEqual(4, shifts.Where(p => !p.Value.IsSmallShift).Count());
        }

    }
}
