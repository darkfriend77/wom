using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoMInterface.Game.Model;

namespace WoMInterface.Node.Tests
{
    [TestClass]
    public class BlockchainTest
    {

        [TestMethod]
        public void TryGetBlockHashesTest()
        {
            var instance = new Blockchain("http://127.0.0.1:17710", "mogwai","mogwai","");
            Assert.IsTrue(instance.TryGetBlockHashes(1, 3, null, out Dictionary<int,string> testDic1));
            Assert.AreEqual(2, testDic1.Count);

            Assert.IsTrue(testDic1.TryGetValue(1, out string value11));
            Assert.AreEqual("000004a3418bf6f7a085b0a489d56eea4fbc094be8ec48ad7ec11621a4dd7431", value11);
            Assert.IsTrue(testDic1.TryGetValue(2, out string value12));
            Assert.AreEqual("00000c6aa9af627b13ecba115a82e61b1188f97ca1f446b1aae16c90748038e4", value12);

            Assert.IsTrue(instance.TryGetBlockHashes(1, 3, new string[] { "000004a3418b" }, out Dictionary<int, string> testDic2));
            Assert.IsTrue(testDic2.TryGetValue(1, out string value21));
            Assert.AreEqual("000004a3418bf6f7a085b0a489d56eea4fbc094be8ec48ad7ec11621a4dd7431", value21);
            Assert.AreEqual(1, testDic2.Count);

            Assert.IsTrue(instance.TryGetBlockHashes(1, 3, new string[] { "000004a3418b", "f97ca1f446b1aae" }, out Dictionary<int, string> testDic3));
            Assert.IsTrue(testDic3.TryGetValue(1, out string value31));
            Assert.AreEqual("000004a3418bf6f7a085b0a489d56eea4fbc094be8ec48ad7ec11621a4dd7431", value31);
            Assert.IsTrue(testDic3.TryGetValue(2, out string value32));
            Assert.AreEqual("00000c6aa9af627b13ecba115a82e61b1188f97ca1f446b1aae16c90748038e4", value32);

        }
    }
}
