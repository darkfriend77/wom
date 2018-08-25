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
        public DifficultyType Difficulty { get; }

        public int AveragePartyLevel { get; }

        public int ChallengeRating => AveragePartyLevel + (int) Difficulty;

        public Adventure(DifficultyType difficulty, int averagePartyLevel) : base(InteractionType.ADVENTURE)
        {
            Difficulty = difficulty;
            AveragePartyLevel = averagePartyLevel;

        }
    }
}
