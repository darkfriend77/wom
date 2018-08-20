using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoMInterface.Tool;

namespace WoMInterface.Game.Tests
{
    [TestClass]
    public class AttributeTest
    {
        [TestMethod]
        public void GenderTest()
        {
            Attribute GenderAttr = AttributBuilder.Create("Gender")
                .Salted(false).Position(2).Size(1).Creation(2).MaxRange(2).Build();

            Dictionary<int, int> dict = new Dictionary<int, int>();
            foreach (char b1 in Base58Encoding.Digits.ToCharArray())
            {
                foreach (char b2 in Base58Encoding.Digits.ToCharArray())
                {
                    string addr = "M" + b1 + b2 + "KtKS3AeNuRFWE5Qj9tFiNAahWvQMTiz";
                    var pubMogAddressHex = HexHashUtil.ByteArrayToString(Base58Encoding.Decode(addr));
                    HexValue hexValue = new HexValue(
                                        new Shift()
                                        {
                                            Time = 1530914381,
                                            BkIndex = 2,
                                            Amount = 1,
                                            Height = 7234,
                                            AdHex = pubMogAddressHex,
                                            BkHex = "00000000090d6c6b058227bb61ca2915a84998703d4444cc2641e6a0da4ba37e",
                                            TxHex = "163d2e383c77765232be1d9ed5e06749a814de49b4c0a8aebf324c0e9e2fd1cf"
                                        });

                    GenderAttr.CreateValue(hexValue);

                    int value = GenderAttr.GetValue();
                    int orgValue = HexHashUtil.GetHexVal(pubMogAddressHex[1]);
                    if (dict.TryGetValue(value, out int count))
                    {
                        dict[value] = count + 1;
                    }
                    else
                    {
                        dict.Add(value, 1);
                    }
                }
            }

            Assert.AreEqual(2, dict.Count);
            var enumerator = dict.Keys.GetEnumerator();
            enumerator.MoveNext();
            Assert.AreEqual(1, enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual(0, enumerator.Current);
            Assert.AreEqual(1692, dict[0]);
            Assert.AreEqual(1672, dict[1]);
        }
    }
}
