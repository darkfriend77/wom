using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMInterface.Tool
{
    public class Test
    {
        static Random random = new Random();

        public static string GetRandomHexNumber(int digits)
        {
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }

        public static double GetProbability(string str, int digits, double cycles=1000)
        {
            double count = 0;
            for (int i = 0; i < cycles; i++)
            {
                if (GetRandomHexNumber(digits).Contains(str))
                {
                    count++;
                }
            }

            return count / cycles;
        } 
    }
}
