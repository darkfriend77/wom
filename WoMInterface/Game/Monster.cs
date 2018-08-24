using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMInterface.Game
{
    class Monster : Entity
    {
        public enum MonsterType {
            ABERRATIONS, ANIMALS, CONSTRUCTS, DRAGONS, FEY, HUMANOIDS, MAGICALBEASTS, OOZES, OUTSIDERS, PLANTS, UNDEAD, VERMIN
        }

        public Monster(Dice dice, int level, MonsterType monsterType)
        {
            // create abilities
            Gender = dice.Roll(2, -1);
            Strength = dice.Roll(2);
            Dexterity = dice.Roll(2);
            Constitution = dice.Roll(2);
            Inteligence = dice.Roll(2);
            Wisdom = dice.Roll(2);
            Charisma = dice.Roll(2);

            HitPointDice = 8;
            HitPointLevelRolls = dice.Roll(new int[] { level-1, HitPointDice, level-1});
            Equipment.PrimaryWeapon = new Weapon()
            {
                DamageRoll = new int[] { 1, 6, 1 }
            };

        }
    }
}
