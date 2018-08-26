using System;
using WoMInterface.Game.Interaction;
using WoMInterface.Game.Model;
using WoMInterface.Game.Random;
using WoMInterface.Game.Combat;

namespace WoMInterface.Game.Generator
{
    public class AdventureGenerator
    {
        public static Adventure Create(Shift generatorShift, AdventureAction adventureAction, Mogwai mogwai)
        {
            switch (adventureAction.AdventureType)
            {
                case Enums.AdventureType.TEST_ROOM:
                    return CreateTestRoom(adventureAction.ChallengeRating, mogwai, generatorShift);
                default:
                    throw new NotImplementedException();
            }
        }

        private static TestRoom CreateTestRoom(int challengeRating, Mogwai mogwai, Shift shift)
        {
            Dice monsterDice = new Dice(shift, 1);
            SimpleFight simpleFight = new SimpleFight(mogwai, shift.MogwaiDice, new Rat(monsterDice), monsterDice);
            TestRoom testRoom = new TestRoom(simpleFight);
            return testRoom;
        }
    }
}
