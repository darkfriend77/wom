using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game.Enums;

namespace WoMInterface.Game.Interaction
{

    public class Adventure : Interaction
    {
        public AdventureType AdventureType { get; }

        public DifficultyType Difficulty { get; }

        public int AveragePartyLevel { get; }

        public int ChallengeRating => AveragePartyLevel + (int) Difficulty;

        public Adventure(AdventureType adventureType, DifficultyType difficulty, int averagePartyLevel) : base(InteractionType.ADVENTURE)
        {
            AdventureType = adventureType;
            Difficulty = difficulty;
            AveragePartyLevel = averagePartyLevel;
            ParamAdd1 = ((int)AdventureType * 1000) + ChallengeRating;
            ParamAdd2 = 1234;
        }

    }
}
