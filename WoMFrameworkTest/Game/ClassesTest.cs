using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoMFramework.Game.Model;

namespace WoMFramework.Game.Model.Tests
{
    [TestClass]
    public class ClassesTest
    {
        [TestMethod]
        public void NoClasstest()
        {
            NoClass noClass = new NoClass();
            Assert.AreEqual(1, noClass.BaseAttackBonus.Length);
            Assert.AreEqual(0, noClass.BaseAttackBonus[0]);
            Assert.AreEqual(0, noClass.ClassLevel);
            noClass.ClassLevelUp();
            Assert.AreEqual(1, noClass.ClassLevel);
        }

        [TestMethod]
        public void BarbarianTest()
        {

            Barbarian barbarian = new Barbarian();

            Assert.AreEqual(1, barbarian.BaseAttackBonus.Length);
            Assert.AreEqual(0, barbarian.BaseAttackBonus[0]);
            Assert.AreEqual(0, barbarian.ClassLevel);
            Assert.AreEqual(0, barbarian.FortitudeBaseSave);
            Assert.AreEqual(0, barbarian.ReflexBaseSave);
            Assert.AreEqual(0, barbarian.WillBaseSave);
            barbarian.ClassLevelUp();
            Assert.AreEqual(1, barbarian.BaseAttackBonus.Length);
            Assert.AreEqual(1, barbarian.BaseAttackBonus[0]);
            Assert.AreEqual(1, barbarian.ClassLevel);
            Assert.AreEqual(2, barbarian.FortitudeBaseSave);
            Assert.AreEqual(0, barbarian.ReflexBaseSave);
            Assert.AreEqual(0, barbarian.WillBaseSave);
            barbarian.ClassLevelUp();
            Assert.AreEqual(1, barbarian.BaseAttackBonus.Length);
            Assert.AreEqual(2, barbarian.BaseAttackBonus[0]);
            Assert.AreEqual(2, barbarian.ClassLevel);
            Assert.AreEqual(3, barbarian.FortitudeBaseSave);
            Assert.AreEqual(0, barbarian.ReflexBaseSave);
            Assert.AreEqual(0, barbarian.WillBaseSave);
            barbarian.ClassLevelUp();
            Assert.AreEqual(1, barbarian.BaseAttackBonus.Length);
            Assert.AreEqual(3, barbarian.BaseAttackBonus[0]);
            Assert.AreEqual(3, barbarian.ClassLevel);
            Assert.AreEqual(3, barbarian.FortitudeBaseSave);
            Assert.AreEqual(1, barbarian.ReflexBaseSave);
            Assert.AreEqual(1, barbarian.WillBaseSave);
            barbarian.ClassLevelUp();
            Assert.AreEqual(1, barbarian.BaseAttackBonus.Length);
            Assert.AreEqual(4, barbarian.BaseAttackBonus[0]);
            Assert.AreEqual(4, barbarian.ClassLevel);
            Assert.AreEqual(4, barbarian.FortitudeBaseSave);
            Assert.AreEqual(1, barbarian.ReflexBaseSave);
            Assert.AreEqual(1, barbarian.WillBaseSave);
            barbarian.ClassLevelUp();
            Assert.AreEqual(1, barbarian.BaseAttackBonus.Length);
            Assert.AreEqual(5, barbarian.BaseAttackBonus[0]);
            Assert.AreEqual(5, barbarian.ClassLevel);
            Assert.AreEqual(4, barbarian.FortitudeBaseSave);
            Assert.AreEqual(1, barbarian.ReflexBaseSave);
            Assert.AreEqual(1, barbarian.WillBaseSave);
            barbarian.ClassLevelUp();
            Assert.AreEqual(2, barbarian.BaseAttackBonus.Length);
            Assert.AreEqual(6, barbarian.BaseAttackBonus[0]);
            Assert.AreEqual(1, barbarian.BaseAttackBonus[1]);
            Assert.AreEqual(6, barbarian.ClassLevel);
            Assert.AreEqual(5, barbarian.FortitudeBaseSave);
            Assert.AreEqual(2, barbarian.ReflexBaseSave);
            Assert.AreEqual(2, barbarian.WillBaseSave);
            barbarian.ClassLevelUp();
            Assert.AreEqual(2, barbarian.BaseAttackBonus.Length);
            Assert.AreEqual(7, barbarian.BaseAttackBonus[0]);
            Assert.AreEqual(2, barbarian.BaseAttackBonus[1]);
            Assert.AreEqual(7, barbarian.ClassLevel);
            barbarian.ClassLevelUp();
            Assert.AreEqual(2, barbarian.BaseAttackBonus.Length);
            Assert.AreEqual(8, barbarian.BaseAttackBonus[0]);
            Assert.AreEqual(3, barbarian.BaseAttackBonus[1]);
            Assert.AreEqual(8, barbarian.ClassLevel);
            barbarian.ClassLevelUp();
            Assert.AreEqual(2, barbarian.BaseAttackBonus.Length);
            Assert.AreEqual(9, barbarian.BaseAttackBonus[0]);
            Assert.AreEqual(4, barbarian.BaseAttackBonus[1]);
            Assert.AreEqual(9, barbarian.ClassLevel);
            barbarian.ClassLevelUp();
            Assert.AreEqual(2, barbarian.BaseAttackBonus.Length);
            Assert.AreEqual(10, barbarian.BaseAttackBonus[0]);
            Assert.AreEqual(5, barbarian.BaseAttackBonus[1]);
            Assert.AreEqual(10, barbarian.ClassLevel);
            barbarian.ClassLevelUp();
            Assert.AreEqual(3, barbarian.BaseAttackBonus.Length);
            Assert.AreEqual(11, barbarian.BaseAttackBonus[0]);
            Assert.AreEqual(6, barbarian.BaseAttackBonus[1]);
            Assert.AreEqual(1, barbarian.BaseAttackBonus[2]);
            Assert.AreEqual(11, barbarian.ClassLevel);
        }
    }
}
