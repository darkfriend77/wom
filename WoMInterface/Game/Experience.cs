using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Tool;

namespace WoMInterface.Game
{
    public class Experience
    {
        private string[] expPats;

        public double Exp { get; private set; } = 0;

        public double GetLevel(double exp) => Math.Floor(0.05 + Math.Pow(exp, 0.05) * Math.Pow(exp, 0.27));

        public double CurrentLevel => GetLevel(Exp);

        public Experience(Shift shift)
        {
            expPats = GetExpPatterns(shift.TxHex);
        }

        public Experience()
        {
        }

        public void Add(double exp)
        {
            Exp += exp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txHex"></param>
        /// <returns></returns>
        public string[] GetExpPatterns(string txHex)
        {
            var expPatterns = txHex.Replace('0', '8');
            var expPats = new List<string>();
            for (int i = 0; i + 4 <= 44; i = i + 4)
            {
                expPats.Add(expPatterns.Substring(i, 4));
            }
            for (int i = 44; i + 3 <= 62; i = i + 3)
            {
                expPats.Add(expPatterns.Substring(i, 3));
            }
            for (int i = 62; i + 2 <= 64; i = i + 2)
            {
                expPats.Add(expPatterns.Substring(i, 2));
            }
            return expPats.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shift"></param>
        internal void LazyExperience(Shift shift)
        {
            int hexSize = shift.BkHex.Length;
            int lazyExpLevel = (int)CurrentLevel / 10;

            for (int i = 0; i <= lazyExpLevel; i++)
            {
                string exPat = expPats[i % 18];
                int indExp = shift.BkHex.IndexOf(exPat);
                if (indExp != -1)
                {
                    var charMultiplierA = shift.BkHex[(hexSize + indExp - 1)% hexSize];
                    var charMultiplierB = shift.BkHex[(indExp + exPat.Length) % hexSize];
                    var exp = HexHashUtil.GetHexVal(charMultiplierA) * HexHashUtil.GetHexVal(charMultiplierB);
                    Add(exp);
                }
            }
        }

        internal void Print()
        {
            Console.WriteLine("- Experience:");
            Console.WriteLine(string.Join(";", expPats));
        }
    }
}
