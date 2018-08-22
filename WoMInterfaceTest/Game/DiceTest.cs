using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoMInterface.Game;

namespace WoMInterface.Game.Tests
{
    [TestClass]
    public class DiceTest
    {
        [TestMethod]
        public void RollDiceSimple()
        {
            Shift shift = new Shift()
            {
                Time = 1530914381,
                BkIndex = 2,
                Amount = 1,
                Height = 7234,
                AdHex = "32ad9e02792599dfdb6a9d0bc0b924da23bd96b1b7eb4f0a68",
                BkHex = "00000000090d6c6b058227bb61ca2915a84998703d4444cc2641e6a0da4ba37e",
                TxHex = "163d2e383c77765232be1d9ed5e06749a814de49b4c0a8aebf324c0e9e2fd1cf"
            };

            Dice dice = new Dice(shift);
            Dictionary<int,int> ProbabilityDict = new Dictionary<int, int>();

            int n = 1000000;

            for (int i = 0; i < 20 * n; i++)
            {
                int roll = dice.Roll(20);
                if (ProbabilityDict.TryGetValue(roll, out int count))
                {
                    ProbabilityDict[roll] = count + 1;
                }
                else
                {
                    ProbabilityDict[roll] = 1;
                }
            }

            foreach(var keyValue in ProbabilityDict)
            {
                Assert.IsTrue(keyValue.Value > 0.8 * n && keyValue.Value < 1.2 * n);
            }

        }

        [TestMethod]
        public void RollDiceEvent()
        {
            Shift shift = new Shift()
            {
                Time = 1530914381,
                BkIndex = 2,
                Amount = 1,
                Height = 7234,
                AdHex = "32ad9e02792599dfdb6a9d0bc0b924da23bd96b1b7eb4f0a68",
                BkHex = "00000000090d6c6b058227bb61ca2915a84998703d4444cc2641e6a0da4ba37e",
                TxHex = "163d2e383c77765232be1d9ed5e06749a814de49b4c0a8aebf324c0e9e2fd1cf"
            };

            Dice dice = new Dice(shift);
            int n = 1000000;
            int[] rollEvent = new int[] { 4, 6, 3 };
            for (int i = 0; i < 20 * n; i++)
            {
                int roll = dice.Roll(rollEvent);
                Assert.IsTrue(roll < 19 && roll > 2);
            }

        }
    }
}
