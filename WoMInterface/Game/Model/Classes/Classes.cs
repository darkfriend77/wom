using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMInterface.Game.Model
{
    public abstract class Classes
    {
        public int ClassLevel { get; set; }

        public int[] BaseAttackBonus { get; set; }

        public Classes()
        {
            ClassLevel = 1;
            BaseAttackBonus = new int[] { 0 };
        }

        //internal void AddBaseAttackBonus(int value)
        //{
        //    int currentBaseAttackBonus = BaseAttackBonus[0] + value;
        //    int babCount = currentBaseAttackBonus / 5;

        //    if (currentBaseAttackBonus % 5 == 0 && babCount > 0)
        //    {
        //        babCount -= 1;
        //    }

        //    int[] result = new int[babCount];
        //    for (int i = 0; i < result.Length; i++)
        //    {
        //        result[i] = currentBaseAttackBonus;
        //        currentBaseAttackBonus -= 5;
        //    }

        //    BaseAttackBonus = result;
        //}

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
