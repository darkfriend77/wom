using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoMInterface.Game;
using WoMInterface.Game.Interaction;
using WoMInterface.Game.Random;

namespace WoMInterface.Game.Tests
{
    [TestClass]
    public class DiceTest
    {
        [TestMethod]
        public void RollDiceSimple()
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
                Assert.IsTrue(keyValue.Value > 0.9 * n && keyValue.Value < 1.1 * n);
            }

        }

        [TestMethod]
        public void RollDiceModifierSimple()
        {
            Shift shift = new Shift(0D)
            {
                Time = 1531171420,
                BkIndex = 11,
                Amount = 1,
                Height = 9196,
                AdHex = "32f13027e869de56de3c2d5af13f572b67b5e75a18594013ec",
                BkHex = "000000001f2ade78b094fce0fbfacc55da3a23ec82489171eb2687a1b6582d12",
                TxHex = "9679a3d39efdf8faa019410250fa91647a76cbb1bd2fd1c5d7ba80551b4edd7b"
            };

            Dice dice = new Dice(shift, 2);
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
                Assert.IsTrue(keyValue.Value > 0.9 * n && keyValue.Value < 1.1 * n);
            }

        }

        [TestMethod]
        public void RollDiceEvent()
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
