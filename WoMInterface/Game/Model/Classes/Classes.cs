﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMInterface.Game.Model
{
    public abstract class Classes
    {
        public int ClassLevel { get; set; }

        public int FortitudeBaseSave { get; set; }
        public int ReflexBaseSave { get; set; }
        public int WillBaseSave { get; set; }

        public int[] BaseAttackBonus { get; set; }
        
        public Classes()
        {
            ClassLevel = 1;
            BaseAttackBonus = new int[] { 0 };
        }

        internal void AddBaseAttackBonus(int value)
        {
            int currentBaseAttackBonus = BaseAttackBonus[0] + value;

            var baseAttackBonusList = new List<int>();

            for (int i = currentBaseAttackBonus; i > 0; i = i - 5) {
                baseAttackBonusList.Add(i);
            }
            BaseAttackBonus = baseAttackBonusList.ToArray();
        }

        public virtual void LevelUp()
        {
            ClassLevel += 1;
        }

    }
}
