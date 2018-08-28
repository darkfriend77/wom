using System;
using System.Collections.Generic;
using WoMInterface.Game.Enums;

namespace WoMInterface.Game.Model
{
    public sealed class WeaponBuilder
    {
        // description
        private int criticalMinRoll = 20;
        private int criticalMultiplier = 2;
        private WeaponDamageType[] weaponDamageTypes = new WeaponDamageType[] { WeaponDamageType.BLUDGEONING, WeaponDamageType.PIERCING, WeaponDamageType.SLASHING };
        private int weight = 1;
        private string description = string.Empty;

        public string name;
        public int[] damageRollEvent;

        private WeaponBuilder(string name, int[] damageRollEvent)
        {
            this.name = name;
            this.damageRollEvent = damageRollEvent;
        }
        public static WeaponBuilder Create(string name, int[] damageRollEvent)
        {
            return new WeaponBuilder(name, damageRollEvent);
        }
        public WeaponBuilder SetCriticalMinRoll(int criticalMinRoll)
        {
            this.criticalMinRoll = criticalMinRoll;
            return this;
        }
        public WeaponBuilder SetCriticalMultiplier(int criticalMultiplier)
        {
            this.criticalMultiplier = criticalMultiplier;
            return this;
        }
        public WeaponBuilder SetDamageType(WeaponDamageType weaponDamageType)
        {
            this.weaponDamageTypes = new WeaponDamageType[] { weaponDamageType };
            return this;
        }
        public WeaponBuilder SetDamageTypes(WeaponDamageType[] weaponDamageTypes)
        {
            this.weaponDamageTypes = weaponDamageTypes;
            return this;
        }
        public WeaponBuilder SetDescription(string description)
        {
            this.description = description;
            return this;
        }
        public Weapon Build()
        {
            return new Weapon(name, damageRollEvent, criticalMinRoll, criticalMultiplier, weaponDamageTypes, weight)
            {
                Description = description
            };
        }
    }
    public class NaturalWeapon
    {
        private static Dictionary<SizeType, int[]> BiteDic = new Dictionary<SizeType, int[]>() {
            { SizeType.DIMINUTIVE, new int[] {1, 2} },
            { SizeType.TINY, new int[] {1, 3} },
            { SizeType.SMALL, new int[] {1, 4} },
            { SizeType.MEDIUM, new int[] {1, 6} },
            { SizeType.LARGE, new int[] {1, 8} },
            { SizeType.HUGE, new int[] {2, 6} },
            { SizeType.GARGANTUAN, new int[] {2, 8} },
            { SizeType.COLOSSAL, new int[] {4, 6} },
        };

        public static Weapon Bite(SizeType sizeType)
        {
            return new Weapon("Bite", BiteDic[sizeType], 20, 2, new WeaponDamageType[] { WeaponDamageType.BLUDGEONING, WeaponDamageType.PIERCING, WeaponDamageType.SLASHING }, 0);
        }
    }
    public class Weapon
    {
        public string Name { get; }
        public int[] DamageRoll { get; }
        public int CriticalMinRoll { get; }
        public int CriticalMultiplier { get; }
        public WeaponDamageType[] WeaponDamageTypes { get; }
        public double Weight { get; }
        public string Description { get; set; }
        public bool IsCriticalRoll(int roll) => roll >= CriticalMinRoll;
        
        public Weapon(string name, int[] damageRoll, int criticalMinRoll, int criticalMultiplier, WeaponDamageType[] weaponDamageTypes, double weight)
        {
            Name = name;
            DamageRoll = damageRoll;
            CriticalMinRoll = criticalMinRoll;
            CriticalMultiplier = criticalMultiplier;
            WeaponDamageTypes = weaponDamageTypes;
            Weight = weight;
        }

    }

}