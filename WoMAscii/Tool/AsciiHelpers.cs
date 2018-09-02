using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMAscii.Tool
{
    public class AsciiHelpers
    {
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

        public static void Msg(string message)
        {
            // this is a ¬Rmessage§ red info => message red
            // ¬ last sign           => nextline
            if (message.Length == 0)
            {
                return;
            }

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

        public static string GetBar(double value1, double value2, out int hpPerc)
        {
            var rate = value1 / value2;
            hpPerc = (int) (rate * 100);
            int count = (int) (15 * rate);
            return "¬G"+ string.Empty.PadRight(count,'o') + "§" + "¬R"+ string.Empty.PadRight(15-count,'.') + "§";
        }

        private static void ColorWriteLine(string value, ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
        {
            //
            // This method writes an entire line to the console with the string.
            //
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
            Console.WriteLine(value.PadRight(Console.WindowWidth - 1)); // <-- see note
            //
            // Reset the color.
            //
            Console.ResetColor();
        }

        public static void Print(string[] stringArray, ConsoleColor color)
        {
            foreach (var str in stringArray)
            {
                ColorWriteLine(str, color);
            }
        }

        public static void PrintSpecial(string[] stringArray)
        {
            foreach (var str in stringArray)
            {
                Msg(str);
            }
        }
    }
}
