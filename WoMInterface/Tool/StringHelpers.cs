using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMInterface.Tool
{
    class StringHelpers
    {
        public static bool StringContainsStringFromArray(String inputStr, String[] strArray)
        {
            foreach (string str in strArray)
            {
                if (inputStr.Contains(str))
                {
                    return true;
                }
            }
            return false;
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
