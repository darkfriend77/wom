﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMInterface.Game.Model
{
    public class Barbarian : Classes
    {
        public Barbarian()
        {
            BaseAttackBonus = new int[] { 1 };
        }

        public override void LevelUp()
        {
            base.LevelUp();

            FortitudeBaseSave += 1;
            ReflexBaseSave += 1;
            WillBaseSave += 1;

            AddBaseAttackBonus(1);
        }
    }
}
