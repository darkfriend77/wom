using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoMInterface.Game.Enums;

namespace WoMInterface.Game.Interaction.Tests
{
    [TestClass]
    public class InteractionTest
    {
        [TestMethod]
        public void InteractionSerialisation()
        {
            //decimal amount = 0.12345678m;
            //decimal fee = 0.00011234m;
            //string parm1 = (amount - fee).ToString("0.00000000").Split('.')[1];
            //string saveParm = fee.ToString("0.00000000").Split('.')[1].Substring(4);
            //string costType = parm1.Substring(0, 2);
            //string actionType = parm1.Substring(2, 2);
            //string addParm = parm1.Substring(4, 4);


            Adventure adventure1 = new Adventure(AdventureType.TEST_ROOM, DifficultyType.CHALLENGING, 2);
            Assert.AreEqual(0.01040003m, adventure1.GetValue1());
            Assert.AreEqual(0.00001002m, adventure1.GetValue2());

            Adventure adventure2 = (Adventure) Interaction.GetInteraction(adventure1.GetValue1() + adventure1.GetValue2(), adventure1.GetValue2());
            Assert.AreEqual(0.01040003m, adventure2.GetValue1());
            Assert.AreEqual(0.00001002m, adventure2.GetValue2());

            //Console.WriteLine($"Value1: {adventure.CostType}");
            //Console.WriteLine($"Value1: {adventure.InteractionType}");
            //Console.WriteLine($"Value1: {adventure.AdventureType}");
            //Console.WriteLine($"Value1: {adventure.ChallengeRating}");
            //Console.WriteLine($"Value1: {adventure.DifficultyType}");
            //Console.WriteLine($"Value1: {adventure.AveragePartyLevel}");
            //Console.WriteLine($"Value1: {adventure.ParamAdd1}");
            //Console.WriteLine($"Value2: {adventure.ParamAdd2}");
        }
    }
}
