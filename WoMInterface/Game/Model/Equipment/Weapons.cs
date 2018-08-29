using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game.Enums;

namespace WoMInterface.Game.Model
{
    public class Weapons
    {
        /***
         * Unarmed Weapons
         */
        public static Weapon Gauntlet =>
            WeaponBuilder.Create("Gauntlet", new int[] { 1, 2 }, new int[] { 1, 3 })
            .SetDamageType(WeaponDamageType.BLUDGEONING)
            .Build();


        /***
         * One-Handed Melee Weapons
         */
        public static Weapon Rapier =>
            WeaponBuilder.Create("Rapier", new int[] { 1, 4 }, new int[] { 1, 6 })
            .SetCriticalMinRoll(18)
            .SetDamageType(WeaponDamageType.PIERCING)
            .Build();


        /***
         * Two-Handed Melee Weapons
         */
        public static Weapon Spear =>
            WeaponBuilder.Create("Spear", new int[] { 1, 6 }, new int[] { 1, 8 })
            .IsTwoHanded()
            .SetCriticalMultiplier(3)
            .SetDamageType(WeaponDamageType.PIERCING)
            .Build();


    }
}
