using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game.Enums;
using WoMInterface.Game.Random;
using WoMInterface.Tool;

namespace WoMInterface.Game.Model
{
    public abstract class Entity
    {
        public string Name { get; set; }

        public int Gender { get; set; }
        public string MapGender => ((GenderType)Gender).ToString();

        public SizeType SizeType { get; set; }

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

        private int Modifier(int ability) => (int)Math.Floor((ability - 10) / 2.0);

        // armorclass = 10 + armor bonus + shield bonus + dex modifier + size modifier + natural armor + deflection + misc modifier
        public int ArmorClass => 10 + Equipment.ArmorBonus + Equipment.ShieldBonus + DexterityMod + (int) SizeType + NaturalArmor;
        public int NaturalArmor { get; set; }

        // hitpoints
        public int HitPointDice { get; set; }
        public int[] HitPointDiceRollEvent { get; set; }
        public List<int> HitPointLevelRolls { get; }
        public int MaxHitPoints => HitPointDice + HitPointLevelRolls.Sum();
        public int CurrentHitPoints { get; set; }

        // initiative = dex modifier + misc modifier
        public int Initiative => DexterityMod;

        // base attack bonus = class dependent value
        public int BaseAttackBonus { get; set; }

        // attackbonus = base attack bonus + strength modifier + size modifier
        public int AttackBonus => BaseAttackBonus + StrengthMod + (int) SizeType;

        // attack roll
        public int[] AttackRolls(Dice dice, int criticalMinRoll = 21)
        {
            int lastRoll = 0;
            List<int> rolls = new List<int>();
            for (int i = 0; i < 3; i++) {
                lastRoll = dice.Roll(DiceType.D20);
                rolls.Add(lastRoll + AttackBonus);
                if (lastRoll < criticalMinRoll)
                    break;
            }
            return rolls.ToArray();
        }

        // initiative roll
        public int InitiativeRoll(Dice dice) => dice.Roll(DiceType.D20) + Initiative;

        // damage
        public int DamageRoll(Dice dice)
        {
            int damage = dice.Roll(Equipment.PrimaryWeapon.DamageRoll) + StrengthMod;
            return damage < 1 ? 1 : damage;
        }

        // injury and death
        public HealthState HealthState
        {
            get
            {
                if (CurrentHitPoints == MaxHitPoints)
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

        // equipment
        public Equipment Equipment { get; }

        // dice
        public virtual Dice Dice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Entity()
        {
            // initialize
            HitPointLevelRolls = new List<int>();
            Equipment = new Equipment();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        internal void Attack(int turn, Entity target)
        {
            Weapon weapon = Equipment.PrimaryWeapon;

            int[] attackRolls = AttackRolls(Dice, weapon.CriticalMinRoll);
            int attack = AttackRoll(attackRolls, target.ArmorClass, out int criticalCounts);

            string attackStr = criticalCounts > 0 ? "critical" : attack.ToString();
            StringHelpers.Msg($" + ¬g{turn.ToString("00")}§: ¬C{Name}§[¬G{CurrentHitPoints}§] attacks ¬C{target.Name}§ with ¬c{weapon.Name}§ roll ¬Y{attackStr}§[¬a{target.ArmorClass}§]:");

            if (attack > target.ArmorClass || criticalCounts > 0)
            {

                int damage = DamageRoll(Dice);
                int criticalDamage = 0;
                if (criticalCounts > 0)
                {
                    for (int i = 0; i < weapon.CriticalMultiplier-1; i++)
                    {
                        criticalDamage += DamageRoll(Dice); 
                    }
                }
                string criticalStr = criticalDamage > 0 ? $"¬y(+{criticalDamage})§" : string.Empty;
                StringHelpers.Msg($" hit for ¬y{damage}§{criticalStr} damage!¬");
                target.Damage(damage + criticalDamage, DamageType.WEAPON);
            }
            else
            {
                StringHelpers.Msg($" ¬Rfailed§!¬");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attackRolls"></param>
        /// <param name="armorClass"></param>
        /// <param name="criticalCount"></param>
        /// <returns></returns>
        private int AttackRoll(int[] attackRolls, int armorClass, out int criticalCount)
        {
            int attack = attackRolls[attackRolls.Length - 1];
            if (attack > armorClass)
            {
                criticalCount = attackRolls.Length - 1;
                return attack;
            }
            criticalCount = attackRolls.Length > 2 ? attackRolls.Length - 2 : 0;
            return attack;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="healAmount"></param>
        /// <param name="healType"></param>
        public void Heal(int healAmount, HealType healType)
        {
            int missingHealth = MaxHitPoints - CurrentHitPoints;
            if (missingHealth <= 0 || healAmount <= 0)
            {
                return;
            }

            if (missingHealth < healAmount)
            {
                healAmount = missingHealth;
            }

            StringHelpers.Msg($"¬C{Name}§ restores ¬G{healAmount}§ HP from {healType.ToString().ToLower()} healing.¬");
            CurrentHitPoints += healAmount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="damageAmount"></param>
        /// <param name="damageType"></param>
        public void Damage(int damageAmount, DamageType damageType)
        {
            if (damageAmount <= 0)
            {
                return;
            }

            StringHelpers.Msg($"¬C{Name}§ suffers ¬R{damageAmount}§ HP from {damageType.ToString().ToLower()} damage.¬");
            CurrentHitPoints -= damageAmount;

            if (CurrentHitPoints < 1)
            {
                StringHelpers.Msg($"¬C{Name}§ got a deadly hit, healthstate is ¬R{HealthState.ToString().ToLower()}§.¬");
            }
        }
    }
}
