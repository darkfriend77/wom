using log4net;
using System;
using System.Reflection;
using WoMAscii.Tool;
using WoMInterface.Game.Ascii;
using WoMInterface.Game.Model;

namespace WoMAscii
{
    internal class WoMGui
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Mogwai mogwai;

        public WoMGui(Mogwai mogwai)
        {
            this.mogwai = mogwai;
        }

        internal void Start()
        {
            try
            {

                ConsoleKey key;
                do
                {
                    Console.Clear();
                    AsciiHelpers.Msg(Panels.CharacterPanel(mogwai));

                    while (!Console.KeyAvailable)
                    {

                    }

                    // Key is available - read it
                    key = Console.ReadKey(true).Key;

                    switch (key)
                    {
                        case ConsoleKey.Enter:
                            Console.Write(".");
                            break;

                        default:
                            break;
                    }

                } while (key != ConsoleKey.Escape);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Terminating commandLine, with exception '{e.Message}'! check log for full exception.");
                Console.WriteLine(e.ToString());
                _log.Error(e.ToString());
                Console.ReadKey();
            }

        }
    }
}