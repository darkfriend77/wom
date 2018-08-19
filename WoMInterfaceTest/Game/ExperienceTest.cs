using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WoMInterface.Game.Tests
{
    [TestClass]
    public class ExperienceTest
    {
        [TestMethod]
        public void GetLevel()
        {
            Experience experience = new Experience();
            Assert.AreEqual(0, experience.CurrentLevel);
            experience.Add(1);
            Assert.AreEqual(1, experience.CurrentLevel);
            experience.Add(8);
            Assert.AreEqual(2, experience.CurrentLevel);
            experience.Add(21);
            Assert.AreEqual(3, experience.CurrentLevel);
            experience.Add(44);
            Assert.AreEqual(4, experience.CurrentLevel);
            experience.Add(75);
            Assert.AreEqual(5, experience.CurrentLevel);
        }
    }
}
