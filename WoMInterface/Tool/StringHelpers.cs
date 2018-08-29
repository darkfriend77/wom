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

        private static ConsoleColor MsgColorMap(string str)
        {
            switch (str)
            {
                case "R":
                    return ConsoleColor.Red;
                case "r":
                    return ConsoleColor.DarkRed;
                case "Y":
                    return ConsoleColor.Yellow;
                case "y":
                    return ConsoleColor.DarkYellow;
                case "G":
                    return ConsoleColor.Green;
                case "g":
                    return ConsoleColor.DarkGreen;
                case "C":
                    return ConsoleColor.Cyan;
                case "c":
                    return ConsoleColor.DarkCyan;
                case "W":
                    return ConsoleColor.White;
                case "S":
                    return ConsoleColor.Black;
                case "B":
                    return ConsoleColor.Blue;
                case "b":
                    return ConsoleColor.DarkBlue;
                case "M":
                    return ConsoleColor.Magenta;
                case "m":
                    return ConsoleColor.DarkMagenta;
                case "A":
                    return ConsoleColor.Gray;
                case "a":
                    return ConsoleColor.DarkGray;
                default:
                    return ConsoleColor.White;
            }
        }
        public static void EvntMsg(string message)
        {
            Msg($"[¬YEVNT§] {message}");
        }
        public static void InfoMsg(string message)
        {
            Msg($"[¬aINFO§] {message}");
        }
        public static void CombMsg(string message)
        {
            Msg($"[¬yCOMB§] {message}");
        }
        public static void HealMsg(string message)
        {
            Msg($"[¬gHEAL§] {message}");
        }
        public static void DamgMsg(string message)
        {
            Msg($"[¬rDAMG§] {message}");
        }
        private static void Msg(string message)
        {
            // this is a ¬Rmessage§ red info => message red
            // ¬ last sign           => nextline

            char last = message[message.Length - 1];
            string[] strArray = message.Split('¬');

            for (int i = 0; i < strArray.Length; i++)
            {
                string currentStr = strArray[i];

                if (currentStr.Length == 0)
                {
                    continue;
                }

                if (i == 0)
                {
                    Console.Write(currentStr);
                    continue;
                }

                var consColor = MsgColorMap(currentStr.Substring(0, 1));
                string[] subStrArray = currentStr.Substring(1).Split('§');
                string colorStr = subStrArray[0];

                if (colorStr.Length > 0)
                {
                    Console.ForegroundColor = consColor;
                    Console.Write(colorStr.PadRight(colorStr.Length - 1));
                    Console.ResetColor();
                }

                if (subStrArray.Length > 1 && subStrArray[1].Length > 0)
                {
                    Console.Write(subStrArray[1].PadRight(subStrArray[1].Length - 1));
                }
            }

            if (last == '¬')
            {
                Console.WriteLine();
            }

        }
    }
}
