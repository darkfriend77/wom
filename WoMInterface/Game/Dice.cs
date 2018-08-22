using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMInterface.Game
{
    public class Dice
    {
        public enum DiceType
        {
            D2, D4, D6, D8, D12, D20, D100
        } 

        private int i1 = 0;

        private int i2 = 0;

        private int i3 = 0;

        private string seed1;

        private string seed2;

        private string seed3;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shift"></param>
        public Dice(Shift shift)
        {
            string height = shift.Height.ToString();
            height = height.PadLeft(height.Length + height.Length % 2, '0');
            this.seed1 = Tool.HexHashUtil.HashSHA256(shift.AdHex + height);
            this.seed2 = Tool.HexHashUtil.HashSHA256(shift.AdHex + shift.BkHex);
            this.seed3 = Tool.HexHashUtil.HashSHA256(height + shift.BkHex);
        }

        public int Roll(int diceSides)
        {
            return (GetNext() % diceSides) + 1;
        }

        public int Roll(int[] rollEvent) {

            var rolls = new List<int>();
            for (int i = 0; i < rollEvent[0]; i++)
            {
                rolls.Add(Roll(rollEvent[1]));
            }

            // best off
            if (rollEvent.Length > 2 && rollEvent[2] > 0)
            {
                var purgeXlowRolls = rollEvent[0] - rollEvent[2];
                for (int j = 0; purgeXlowRolls > 0 && j  < purgeXlowRolls; j++ )
                {
                    rolls.Remove(rolls.Min());
                }
            }

            return rolls.Sum();
        }

        private int GetNext()
        {
            int s1val = Tool.HexHashUtil.GetHexVal(seed1[i1]);
            int s2val = Tool.HexHashUtil.GetHexVal(seed2[i2]);
            int s3val = Tool.HexHashUtil.GetHexVal(seed3[i3]);
            int value = s1val + s2val + s3val;
            i1 = (i1 + 1) % seed1.Length;
            i2 = i1 == 0 ? (i2 + 1) % seed2.Length : i2;
            i3 = i2 == 0 ? (i3 + 1) % seed3.Length : i3;
            return value;
        }

        private int GetSides(DiceType diceType)
        {
            return int.Parse(diceType.ToString().Substring(1));

        }
    }
}
