using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Timers;
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
        private int pointer = 0;
        private int oldPointer = 0;
        private int windowPointer = 0;
        private int maxRows = 20;

        private readonly decimal fees = 0.0001m;

        public void Start()
        {
            XmlConfigurator.Configure();

            try
            {
                Console.WriteLine($"Teleport activated to enter into the World of Mogwais!");
                Console.CursorVisible = false;

                // get deposit address
                string depositAddress = Blockchain.Instance.GetDepositAddress();

                // current un set funds on the wallet.
                decimal unsetFunds = Blockchain.Instance.UnspendFunds(depositAddress, 6);

                // amount of mog that need to be moved minimaly
                int mogAmount = 2;// unsetFunds > 2.0001m ? 2 : 0;

                // response
                string response = " ";

                // initialy updating
                Update();

                // set pointer
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
                    AsciiHelpers.Msg(response, 60);
                    AsciiHelpers.Msg($"¬cDeposit:§ ¬y{depositAddress}§¬");
                    string ufStr = unsetFunds.ToString("#####0.00");
                    string spac = "".PadLeft(9 - ufStr.Length, '_');
                    AsciiHelpers.Msg(Art.Menu(($"¬c{spac}§¬y{ufStr}§¬c_§¬CMOG§¬c_§¬c[§¬G+{mogAmount}§¬c]_§")));
                    AsciiHelpers.Msg(Art.ColumnHeader);
                    PrintMogwais(pointer);
                    AsciiHelpers.Msg(Art.Trailer);

                    var start_time = DateTime.Now;
                    while (!Console.KeyAvailable)
                    {
                        TimeSpan ts = DateTime.Now.Subtract(start_time);
                        if (ts.TotalSeconds > 5)
                        {
                            Console.SetCursorPosition(1, 33);
                            AsciiHelpers.Msg("¬cUpdating§ ¬a... please wait!§");
                            Update();
                            Console.SetCursorPosition(1, 33);
                            Console.Write("".PadLeft(30));
                            Console.SetCursorPosition(1, 33);
                            break;
                        }                       
                    }

                    // reset response
                    response = " ";

                    // no key readed
                    if (!Console.KeyAvailable)
                    {
                        key = 0;
                        continue;
                    }

                    // Key is available - read it
                    key = Console.ReadKey(true).Key;

                    switch (key)
                    {
                        case ConsoleKey.PageUp:
                            mogAmount = mogAmount < 9 ? mogAmount + 1 : mogAmount;
                            break;

                        case ConsoleKey.PageDown:
                            mogAmount = mogAmount > 2 ? mogAmount - 1 : mogAmount;
                            break;

                        case ConsoleKey.DownArrow:
                            if (pointer < mogwais.Count - 1)
                            {
                                pointer++;
                            }
                            break;

                        case ConsoleKey.UpArrow:
                            if (pointer > 0)
                            {
                                pointer--;
                            }
                            break;

                        case ConsoleKey.S:
                            if (unsetFunds > mogAmount + fees)
                            response = Spend(depositAddress, mogwais[pointer].key, mogAmount, fees);
                            unsetFunds = Blockchain.Instance.UnspendFunds(depositAddress, 6);
                            Update();
                            break;

                        case ConsoleKey.C:
                            response = Create();
                            Update();
                            break;

                        case ConsoleKey.T:
                            mogwais[pointer].IsTagged = true;
                            Update();
                            break;

                        case ConsoleKey.B:
                            response = Bind(mogwais[pointer].key);
                            Update();
                            break;

                        case ConsoleKey.P:
                            Play();
                            break;

                        case ConsoleKey.D5:
                            mogwais = mogwais.OrderByDescending(p => p.rateMogwai).ToList();
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

        private void Play()
        {
            var womGui = new WoMGui(mogwais[pointer].mogwai);
            womGui.Start();
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

            int first = windowPointer;
            int last = windowPointer + maxRows - 1;

            for (int i = windowPointer; i < windowPointer + maxRows; i++)
            {
                if (mogwais.Count > i)
                {
                    if (i != 0 && i == first)
                    {
                        AsciiHelpers.Msg($"{mogwais[i]}", 103, "+");
                    }
                    else if (i < mogwais.Count - 1 && i == last )
                    {
                        AsciiHelpers.Msg($"{mogwais[i]}", 103, "+");
                    }
                    else
                    {
                        AsciiHelpers.Msg($"{mogwais[i]}", 103);
                    }
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }

        private string Spend(string fromaddress, string toaddress, decimal burnMogs, decimal fees)
        {
            if (Blockchain.Instance.BurnMogs(fromaddress, toaddress, burnMogs, fees))
            {
                return $"¬GSent§ ¬y{burnMogs}§ ¬Gmogs to the mogwai!§";
            }
            else
            {
                return $"¬YCouldn't send mogs!§";
            };
        }

        private string Create()
        {
            if (Blockchain.Instance.NewMogwaiAddress(out string mogwaiAddress))
            {
                return $"¬GCreated a new address with a mogwai connection!§";
            }
            else
            {
                return $"¬YCouldn't create a new mogwaiaddress, try again!§";
            };
        }

        private string Bind(string address)
        {
            if (Blockchain.Instance.BindMogwai(address))
            {
                return $"¬GBound a mogwai, wait for connection.§";
            }
            else
            {
                return $"¬RCouldn't bind mogwai!§";
            }
        }

        private class MogwaiInfo
        {
            public bool IsSelected { get; set; }
            public bool IsTagged { get; set; }
            public string key;
            public decimal funds;
            public BoundState state;
            public Mogwai mogwai;
            public double rateMogwai = 0;
            public MogwaiInfo(string key, decimal funds, BoundState state, Mogwai mogwai)
            {
                this.key = key;
                this.funds = funds;
                this.state = state;
                this.mogwai = mogwai;
                if (mogwai != null)
                {
                    rateMogwai = (double)
                        (3 * mogwai.Strength 
                        + 3 * mogwai.Dexterity 
                        + 2 * mogwai.Constitution 
                        + 3 * mogwai.Inteligence 
                        + 2 * mogwai.Wisdom 
                        + mogwai.Charisma) / 14;
                }
            }
            public override string ToString()
            {

                string rateMogwaiStr = "...";
                string levelMogwaiStr = "...";
                string goldMogwaiStr = "...";
                if (mogwai != null)
                {
                    rateMogwaiStr = rateMogwai.ToString("#0.00");
                    levelMogwaiStr = mogwai.CurrentLevel.ToString();
                    goldMogwaiStr = mogwai.Wealth.Gold.ToString();
                }
                string stateColor = state == BoundState.NONE ? "a" : state == BoundState.WAIT ? "b" : "y";
                string stateStr = state.ToString().Substring(0, 1) + state.ToString().Substring(1).ToLower();
                string mogwaiName = mogwai != null ? $"{mogwai.Name}" : "...";
                string mCol = mogwai != null ? "y" : "a";
                string selected = IsSelected ? "==>" : "";
                string keyColor = IsTagged ? "g" : IsSelected ? "y" : "a";
                string fundsColor = funds == 0 ? "a" : mogwai != null ? "y" : "W";
                return 
                    $"¬G{selected.PadRight(3).PadLeft(4)}§ " +
                    $"¬{keyColor}{key}§  " +
                    $"¬{stateColor}{stateStr.PadRight(5)}§  " +
                    $"¬{fundsColor}{string.Format("{0:###0.0000}", funds).PadLeft(10).PadRight(11).Substring(0, 11)}§  " +
                    $"¬{mCol}{mogwaiName.PadRight(12)}§  " +
                    $"¬{mCol}{rateMogwaiStr.PadLeft(7).PadRight(8)}§  " +
                    $"¬{mCol}{levelMogwaiStr.PadLeft(6).PadRight(7)}§  " +
                    $"¬{mCol}{goldMogwaiStr.PadLeft(7).PadRight(8)}§" +
                    $"¬";
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
