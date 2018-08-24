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

        public Monster(string name, Dice dice, int level, MonsterType monsterType)
        {
            Name = name;

            // create abilities
            Gender = dice.Roll(2, -1);
            int[] rollEvent = new int[] {4, 6, 3 };
            Strength = dice.Roll(rollEvent);
            Dexterity = dice.Roll(rollEvent);
            Constitution = dice.Roll(rollEvent);
            Inteligence = dice.Roll(rollEvent);
            Wisdom = dice.Roll(rollEvent);
            Charisma = dice.Roll(rollEvent);

            HitPointDice = 8;
            HitPointLevelRolls = dice.Roll(new int[] { level-1, HitPointDice, level-1});
            Equipment.PrimaryWeapon = new Weapon()
            {
                DamageRoll = new int[] { 1, 2, 1 }
            };

            // set moving stats now ....
            Initialize();
        }
    }
}
