using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WoMInterface.Game.Dice;

namespace WoMInterface.Game
{
    public enum GenderType
    {
        MALE,
        FEMALE
    }

    public abstract class Entity
    {
        public string Name { get; set; }

        public int Gender { get; set; }
        public string MapGender => ((GenderType)Gender).ToString();

        public int Strength { get; set; }
        public int StrengthMod => Modifier(Strength);

        public int Dexterity { get; set; }
        public int DexterityMod => Modifier(Dexterity);

        public int Constitution { get; set; }
        public int ConstitutionMod => Modifier(Constitution);

        public int Inteligence { get; set; }
        public int InteligenceMod => Modifier(Inteligence);

        public int Wisdom { get; set; }
        public int WisdomMod => Modifier(Wisdom);

        public int Charisma { get; set; }
        public int CharismaMod => Modifier(Charisma);

        // armorclass = 10 + armor bonus + shield bonus + dex modifier + size modifier + natural armor + deflection + misc modifier
        public int ArmorClass => 10 + DexterityMod;

        // hitpoints
        public int HitPointDice { get; set; } = 0;
        public int HitPointLevelRolls { get; set; } = 0;
        public int HitPoints => HitPointDice + HitPointLevelRolls;

        // initiative = dex modifier + misc modifier
        public int Initiative => DexterityMod;

        // base attack bonus = class dependent value
        public int BaseAttackBonus => 1;

        // attackbonus = base attack bonus + strength modifier + size modifier
        public int AttackBonus => BaseAttackBonus + StrengthMod;

        // damage
        public int Damage(Dice dice) => dice.Roll(Equipment.PrimaryWeapon.DamageRoll) + StrengthMod;

        // equipment
        public Equipment Equipment { get; set; } = new Equipment();

        public Entity()
        {
            
        }

        private int Modifier(int ability)
        {
            return (int)Math.Floor(ability / 10D) - 5;
        }

        public int InitiativeRoll(Dice dice)
        {
            return dice.Roll(DiceType.D20) + Initiative;
        }
        
    }
}
