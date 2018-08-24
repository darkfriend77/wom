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

    public enum HealthState
    {
        HEALTHY = 1,
        INJURED = 0,
        DISABLED = -1,
        DYING = -2,
        DEAD = -3
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
        public int HitPointDice { get; set; }
        public int HitPointLevelRolls { get; set; }
        public int HitPoints => HitPointDice + HitPointLevelRolls;
        private int currentHitPoints;
        public int CurrentHitPoints {
            get => currentHitPoints;
            set => currentHitPoints = value;
        }

        // initiative = dex modifier + misc modifier
        public int Initiative => DexterityMod;

        // base attack bonus = class dependent value
        public int BaseAttackBonus => 1;

        // attackbonus = base attack bonus + strength modifier + size modifier
        public int AttackBonus => BaseAttackBonus + StrengthMod;

        // attack roll
        public int AttackRoll(Dice dice) => dice.Roll(DiceType.D20) + AttackBonus;

        // initiative roll
        public int InitiativeRoll(Dice dice) => dice.Roll(DiceType.D20) + Initiative;

        // damage
        public int DamageRoll(Dice dice) => dice.Roll(Equipment.PrimaryWeapon.DamageRoll) + StrengthMod;

        // injury and death
        public HealthState HealthState
        {
            get
            {
                if (CurrentHitPoints == HitPoints)
                {
                    return HealthState.HEALTHY;
                }
                else if (CurrentHitPoints > 0)
                {
                    return HealthState.INJURED;
                }
                else if (CurrentHitPoints == 0)
                {
                    return HealthState.DISABLED;
                }
                else if (CurrentHitPoints > -10)
                {
                    return HealthState.DYING;
                }
                else
                {
                    return HealthState.DEAD;
                }
            }
        }

        public void Initialize()
        {
            HitPointLevelRolls = 0;
            CurrentHitPoints = HitPoints;
        }

        // equipment
        public Equipment Equipment { get; set; } = new Equipment();

        private int Modifier(int ability)
        {
            return (int) Math.Floor((ability - 10) / 2.0);
        }

    }
}
