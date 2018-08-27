using System;
using WoMInterface.Game.Interaction;
using WoMInterface.Game.Model;
using WoMInterface.Game.Random;
using WoMInterface.Game.Combat;
using System.Collections.Generic;

namespace WoMInterface.Game.Generator
{
    public class AdventureGenerator
    {
        public static Adventure Create(Shift generatorShift, AdventureAction adventureAction)
        {
            switch (adventureAction.AdventureType)
            {
                case Enums.AdventureType.TEST_ROOM:
                    return CreateTestRoom(adventureAction.ChallengeRating);
                default:
                    throw new NotImplementedException();
            }
        }

        private static TestRoom CreateTestRoom(int challengeRatingt)
        {
            SimpleFight simpleFight = new SimpleFight(new List<Monster> {Animals.Rat});
            TestRoom testRoom = new TestRoom(simpleFight);
            return testRoom;
        }
    }
}
