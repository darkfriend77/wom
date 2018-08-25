using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game.Enums;
using WoMInterface.Game.Random;

namespace WoMInterface.Game.Model
{
    public abstract class Monster : Entity
    {
        public int ChallengeRating { get; }

        public MonsterType MonsterType { get; }
        
        public int Experience { get; }

        public Treasure Treasure { get; }

        public string Description { get; set; }

        public Monster(string name, int challengeRating, MonsterType monsterType, int experience, Treasure treasure)
        {
            Name = name;
            ChallengeRating = challengeRating;
            MonsterType = monsterType;
            Experience = experience;
            Treasure = treasure;

        }
    }
}
