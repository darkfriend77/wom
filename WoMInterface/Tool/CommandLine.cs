using BitcoinLib.Responses;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game;
using WoMInterface.Node;

namespace WoMInterface.Tool
{
    class CommandLine
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly string bash = ":> ";
        readonly string[] logo_ascii = new string[] {
            @" __      __            .__       .___         _____     _____                               .__        ",
            @"/  \    /  \___________|  |    __| _/   _____/ ____\   /     \   ____   ______  _  _______  |__| ______",
            @"\   \/\/   /  _ \_  __ \  |   / __ |   /  _ \   __\   /  \ /  \ /  _ \ / ___\ \/ \/ /\__  \ |  |/  ___/",
            @" \        (  <_> )  | \/  |__/ /_/ |  (  <_> )  |    /    Y    (  <_> ) /_/  >     /  / __ \|  |\___ \ ",
            @"  \__/\  / \____/|__|  |____/\____ |   \____/|__|    \____|__  /\____/\___  / \/\_/  (____  /__/____  >",
            @"       \/                         \/                         \/      /_____/              \/        \/ ",
        };
        readonly string[] help_info = new string[] {
            @"+-------------------------------------------------------------- copyright by Mogwaicoin Team 2018 ----+",
            @"|  CommandLine Tool                                                                                   |",
            @"+------------------+----------------------------------------------------------------------------------+",
            @"|  help                  | information about the tool                                                 |",
            @"|  version               | information about the current version                                      |",
            @"|  mogwais               | list of all valid addresses for mogwais and thier state                    |",
            @"|  create                | create a new valid address for a mogwai                                    |",
            @"|  bind <a>              | activate a valid address to bind a mogwai [a=address]                      |",
            @"|  show <a>              | show mogwai data                                                           |",
            @"|  evolve <t>            | ... TODO [t=type]                                                          |",
            @"|  adventure <d> <l>     | ... TODO [d=depth] [l=level]                                               |",
            @"|  loot <o>              | ... TODO [o=option]                                                        |",
            @"|  heatndwater <t>       | ... TODO [t=time]                                                          |",
            @"|  exit                  | leave tool                                                                 |",
            @"|--deprecated commands--------------------------------------------------------------------------------+",
            @"|  settings              | rpc settings for wallet connection (deprecated)                            |",
            @"+-----------------------------------------------------------------------------------------------------+",
            @"|  This tool is only for testing purpose and shouldn't be used by anyone else then mogwaicoin devs.   |",
            @"+-----------------------------------------------------------------------------------------------------+",
        };

        private static readonly Lazy<CommandLine> _lazyInstance = new Lazy<CommandLine>(() => new CommandLine());

        public static CommandLine Instance => _lazyInstance.Value;

        private CommandLine() { }

        public void Start()
        {

            try
            {
                Loop();
            }
            catch (Exception e)
            {
                ConsoleError($"Terminating commandLine, with exception '{e.Message}'! check log for full exception.");
                _log.Error(e.ToString());
                Console.ReadKey();
            }

        }

        public void Loop()
        {
            foreach (string str in logo_ascii)
            {
                ColorWrite(str, ConsoleColor.Cyan);
            }
            foreach (string str in help_info)
            {
                ColorWrite(str, ConsoleColor.DarkCyan);
            }

            // initial caching of the blockchain stuff
            Console.WriteLine("Initial caching of blockhain.");
            Blockchain.Instance.Cache(false, true);

            // put code above while loop that only needs to be executed once
            while (true)
            {
                // get the user input for every iteration, allowing to exit at will
                Console.Write(bash);
                String line = Console.ReadLine();

                if (line.Equals("help"))
                {
                    foreach (string str in help_info)
                    {
                        ColorWrite(str, ConsoleColor.DarkCyan);
                    }
                }
                else if (line.Equals("version"))
                {
                    ConsoleResponse($"Wallet: {Blockchain.Instance.GetInfo().WalletVersion}, CmdLine: {Assembly.GetExecutingAssembly().GetName().Version}");
                }
                else if (line.Equals("settings"))
                {
                    ConsoleResponse($"daemonUrl: '{ConfigurationManager.AppSettings["daemonUrl"]}'");
                    ConsoleResponse($"rpcUsername: '{ConfigurationManager.AppSettings["rpcUsername"]}'");
                    ConsoleResponse($"rpcPassword: '{ConfigurationManager.AppSettings["rpcPassword"]}'");
                    ConsoleResponse($"walletPassword: '{ConfigurationManager.AppSettings["walletPassword"]}'");
                }
                else if (line.Equals("mogwais"))
                {
                    var mogwaiAddressesDict = Blockchain.Instance.ValidMogwaiAddresses();
                    string defaultStr = "+------------------------------------+-------+-------+------------+";
                    ConsoleResponse(defaultStr);
                    ConsoleResponse($"+ Address                            + Bound + Level + Funds      |");
                    ConsoleResponse(defaultStr);
                    foreach (var keyValue in mogwaiAddressesDict)
                    {
                        var key = keyValue.Key;
                        var unspent = Blockchain.Instance.UnspendFunds(key, out List<ListUnspentResponse> listUnspent);
                        var created = Blockchain.Instance.TryGetMogwai(key, out Mogwai mogwai);
                        ConsoleResponse($"| {string.Format("{0,34}", key)} | {string.Format("{0,5}", created)} | {string.Format("{0,5}", mogwai == null ? 0 : mogwai.Experience.CurrentLevel)} | {string.Format("{0:###0.0000}", unspent).PadLeft(10).Substring(0,10)} |");
                        ConsoleResponse(defaultStr);
                    }
                }
                else if (line.Equals("create"))
                {

                    if (Blockchain.Instance.NewMogwaiAddress(out string mogwaiAddress))
                    {
                        ConsoleResponse($"Successfuly, created a new address with a valid mogwai connection! Check with 'mogwais'.");
                    }
                    else
                    {
                        ConsoleWarn($"Couldn't create a new mogwaiaddress, try again!");
                    };
                }
                else if (line.StartsWith("bind"))
                {
                    string[] strArray = line.Split(' ');
                    if (strArray.Count() == 2 && strArray[1].StartsWith("M") && strArray[1].Length == 34)
                    {
                        if (Blockchain.Instance.BindMogwai(strArray[1]))
                        {
                            // TODO
                        }
                        else
                        {
                            ConsoleWarn($"Couldn't bind mogwai!");
                        }
                    }
                    else
                    {
                        ConsoleWarn($"Wrong number of arguments or invalid mogwaiaddress, check help for detailed informations!");
                    }

                }
                else if (line.StartsWith("show"))
                {
                    string[] strArray = line.Split(' ');
                    if (strArray.Count() == 2 && strArray[1].StartsWith("M") && strArray[1].Length == 34)
                    {
                        if (Blockchain.Instance.TryGetMogwai(strArray[1], out Mogwai mogwai) == Blockchain.BoundState.BOUND)
                        {
                            Print(mogwai);
                        }
                        else
                        {
                            ConsoleWarn($"Couldn't show mogwai!");
                        }
                    }
                    else
                    {
                        ConsoleWarn($"Wrong number of arguments or invalid mogwaiaddress, check help for detailed informations!");
                    }

                }
                else if (line.StartsWith("cache"))
                {
                    string[] strArray = line.Split(' ');
                    if (strArray.Count() == 1)
                    {
                        Blockchain.Instance.Cache(false, true);
                    }
                    else if (strArray.Count() == 2)
                    {
                        switch(strArray[1])
                        {
                            case "clear":
                                Blockchain.Instance.Cache(true, true);
                                break;
                            case "stats":
                                Blockchain.Instance.CacheStats();
                                break;
                        }
                    }
                    else
                    {
                        ConsoleWarn($"Wrong number of arguments or invalid command, check help for detailed informations!");
                    }

                }
                else if (line.Equals("exit"))
                {
                    Blockchain.Instance.Exit();
                    return;
                }
                else
                {
                    ConsoleWarn($"Command '{line}' isn't supported, please check 'help' for further guidance.");
                }
                // this is what will happen in the loop body since we didn't exit
                // put whatever note stuff you want to execute again and again in here
            }

        }

        private void Print(Mogwai mogwai)
        {
            string[] template = new string[] {
            @".:-| Name: <name           > [Lvl. <lev>] |-:.  EXP: <exp>".Replace("<name           >", string.Format("{0}", mogwai.Name)).Replace("<lev>", string.Format("{0:####0}", mogwai.Experience.CurrentLevel)).Replace("<exp>", string.Format("{0}", mogwai.Experience.Exp)),
            @"+-stats----------+-------------------+-------------------+",
            @"¦  STR:  <str  > ¦ ALLI: <alli     > ¦ GENDER: <gen    > ¦".Replace("<str  >", string.Format("{0,7}",mogwai.Stats.Strength)).Replace("<alli     >", string.Format("{0,11}",mogwai.Stats.MapAllignment())).Replace("<gen    >", string.Format("{0,9}",mogwai.Body.MapGender())),
            @"¦  CON:  <con  > ¦ LUCK: <luck     > +-------------------+".Replace("<con  >", string.Format("{0,7}",mogwai.Stats.Constitution)).Replace("<con  >", string.Format("{0,11}",mogwai.Stats.Luck)),
            @"¦  DEX:  <dex  > +-------------------+ COATTYPE: <ctype> ¦".Replace("<dex  >", string.Format("{0,7}",mogwai.Stats.Dexterity)).Replace("<ctype>", string.Format("{0,7}",mogwai.Coat.CoatType)),
            @"¦  INT:  <int  > ¦ HP:   <hp       > ¦ COATGEN:  <cgen > ¦".Replace("<int  >", string.Format("{0,7}",mogwai.Stats.Inteligence)).Replace("<cgen >", string.Format("{0,7}",mogwai.Coat.CoatGenetic)),
            @"¦  WIS:  <wis  > ¦ MANA: <mana     > ¦ COLOR1:   <col1 > ¦".Replace("<wis  >", string.Format("{0,7}",mogwai.Stats.Wisdom)).Replace("<col1 >", string.Format("{0,7}",mogwai.Coat.CoatColor1)),
            @"¦  CHA:  <cha  > ¦ END:  <end      > ¦ COLOR2:   <col2 > ¦".Replace("<cha  >", string.Format("{0,7}",mogwai.Stats.Charisma)).Replace("<col2 >", string.Format("{0,7}",mogwai.Coat.CoatColor2)),
            @"+----------------+-------------------+-------------------+",
            @"¦  BOUND: <address                         >             ¦".Replace("<address                         >", string.Format("{0,34}",mogwai.Key)),
            @"+--------------------------------------------------------+" };

            foreach (string str in template)
            {
                ConsoleResponse(str);
            }

            // debug
            //mogwai.Print();
        }

        public void ConsoleWarn(string value)
        {
            ColorWrite(value, ConsoleColor.Yellow);
        }

        public void ConsoleResponse(string value)
        {
            ColorWrite(value, ConsoleColor.Green);
        }

        public void ConsoleError(string value)
        {
            ColorWrite(value, ConsoleColor.Red);
        }

        public void ColorWrite(string value, ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
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
    }
}
