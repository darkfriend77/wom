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
            WeaponBuilder.Create("Gauntlet", new int[] { 1, 2 })
            .SetDamageType(WeaponDamageType.BLUDGEONING)
            .Build();

    }
}
