using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMInterface.Game
{
    public class Dice
    {
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
            this.seed1 = Tool.HexHashUtil.HashSHA256(shift.AdHex + shift.Height);
            this.seed2 = Tool.HexHashUtil.HashSHA256(shift.AdHex + shift.BkHex);
            this.seed3 = Tool.HexHashUtil.HashSHA256(shift.Height + shift.BkHex);
        }

        public int Roll(int diceSides)
        {
            return (GetNext() % diceSides) + 1;
        }

        public int GetNext()
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
    }
}
