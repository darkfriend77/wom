using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WoMAscii.Ascii;
using WoMAscii.Tool;
using WoMInterface.Game.Enums;
using WoMInterface.Game.Model;
using WoMInterface.Node;

namespace WoMAscii
{
    public class NoMoreTyping
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Lazy<NoMoreTyping> _lazyInstance = new Lazy<NoMoreTyping>(() => new NoMoreTyping());

        public static NoMoreTyping Instance => _lazyInstance.Value;

        private NoMoreTyping() { }

        private List<MogwaiInfo> mogwais;
        private int windowPointer = 0;
        private int maxRows = 20;

        public void Start()
        {
            XmlConfigurator.Configure();

            try
            {
                Console.WriteLine($"Press a key to enter into the World of Mogwais! {Blockchain.Instance.GetWalletVersion()}");

                // response
                string response = "¬";

                // initialy updating
                Update();

                // set pointer
                int pointer = 0;
                int oldPointer = 0;
                if (mogwais.Count == 0)
                {
                    pointer = -1;
                    oldPointer = -1;
                }

                ConsoleKey key;
                do
                {
                    if (pointer > -1)
                    {
                        mogwais[oldPointer].IsSelected = false;
                        mogwais[pointer].IsSelected = true;
                    }
                    oldPointer = pointer;

                    Console.Clear();
                    AsciiHelpers.Print(Art.Logo, ConsoleColor.Cyan);
                    AsciiHelpers.PrintSpecial(new string[] { response });
                    AsciiHelpers.PrintSpecial(Art.Menu);
                    PrintMogwais(pointer);
                    AsciiHelpers.PrintSpecial(Art.Trailer);

                    while (!Console.KeyAvailable)
                    {
                        // Do something, but don't read key here
                    }

                    // reset response
                    response = "¬";

                    // Key is available - read it
                    key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.DownArrow && pointer < mogwais.Count - 1)
                    {
                        pointer = pointer + 1;
                    }
                    else if (key == ConsoleKey.UpArrow && pointer > 0)
                    {
                        pointer--;
                    }
                    else if (key == ConsoleKey.C)
                    {
                        response = Create();
                        Update();
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

        private void PrintMogwais(int pointer)
        {
            if (pointer > windowPointer + maxRows - 1)
            {
                windowPointer = pointer - maxRows + 1;
            }
            else if (pointer < windowPointer)
            {
                windowPointer = pointer;
            }

            for (int i = windowPointer; i < windowPointer + maxRows; i++)
            {
                if (mogwais.Count > i)
                {
                    AsciiHelpers.PrintSpecial(new string[] { mogwais[i].ToString() });
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }

        private string Create()
        {
            if (Blockchain.Instance.NewMogwaiAddress(out string mogwaiAddress))
            {
                return $"¬GSuccessfuly, created a new address with a valid mogwai connection!§¬";
            }
            else
            {
                return $"¬YCouldn't create a new mogwaiaddress, try again!§¬";
            };
        }

        private class MogwaiInfo
        {
            public bool IsSelected { get; set; }
            public string key;
            public decimal funds;
            public BoundState state;
            public Mogwai mogwai;
            public MogwaiInfo(string key, decimal funds, BoundState state, Mogwai mogwai)
            {
                this.key = key;
                this.funds = funds;
                this.state = state;
                this.mogwai = mogwai;
            }
            public override string ToString()
            {
                string stateColor = state == BoundState.NONE ? "r" : state == BoundState.WAIT ? "a" : "y";
                string stateStr = state.ToString().Substring(0, 1) + state.ToString().Substring(1).ToLower();
                string mogwaiName = mogwai != null ? $"{mogwai.Name}" : "...";
                string selected = IsSelected ? "==>" : "";
                string keyColor = IsSelected ? "y" : "a";
                string fundsColor = funds > 0 ? "y" : "a";
                return $"¬G{selected.PadRight(3).PadLeft(4)}§ ¬{keyColor}{key}§  ¬{stateColor}{stateStr.PadRight(5)}§  ¬{fundsColor}{string.Format("{0:###0.0000}", funds).PadLeft(10).Substring(0, 10)}§  ¬C{mogwaiName.PadRight(12)}§¬";
            }
        }

        private void Update()
        {
            mogwais = new List<MogwaiInfo>();
            foreach (var address in Blockchain.Instance.ValidMogwaiAddresses().Keys)
            {
                var funds = Blockchain.Instance.UnspendFunds(address);
                var state = Blockchain.Instance.TryGetMogwai(address, out Mogwai mogwai);
                mogwais.Add(new MogwaiInfo(address, funds, state, mogwai));
            }
        }
    }
}
