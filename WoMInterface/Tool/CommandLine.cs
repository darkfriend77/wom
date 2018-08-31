using BitcoinLib.Responses;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using WoMInterface.Game.Model;
using WoMInterface.Node;
using WoMInterface.Game.Random;
using WoMInterface.Game.Interaction;
using WoMInterface.Game.Enums;
using WoMInterface.Game.Combat;
using static WoMInterface.Node.Blockchain;

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
            @"|  mogwais (<s>)         | list of all valid addresses for mogwais and thier state [s=state]          |",
            @"|  create                | create a new valid address for a mogwai                                    |",
            @"|  bind <a>              | activate a valid address to bind a mogwai [a=address]                      |",
            @"|  show <a>              | show mogwai data                                                           |",
            @"|  cache (<o>)           | cache blockchain [o=stats|clear]                                           |",
            @"|  exit                  | leave tool                                                                 |",
            @"|--deprecated commands--------------------------------------------------------------------------------+",
            @"|  settings              | rpc settings for wallet connection (deprecated)                            |",
            @"+-----------------------------------------------------------------------------------------------------+",
            @"|  This tool is only for testing purpose and shouldn't be used by anyone else then mogwaicoin devs.   |",
            @"+-----------------------------------------------------------------------------------------------------+",
        };
        readonly string[] mogwai_control = new string[] {
            @"+-------------------------------------------------------------- copyright by Mogwaicoin Team 2018 ----+",
            @"|  Mogwai Specific Commands                                                                           |",
            @"+------------------+----------------------------------------------------------------------------------+",
            @"|  help                  | show this information again                                                |",
            @"|  dismiss               | dismiss current mogwai                                                     |",
            @"|  update (<o>)          | update current mogwai [o=restart|shift]                                    |",
            @"|  show                  | show current mogwai data                                                   |",
            @"|  adventure <t> <d>     | sent current mogwai to an adventure [t=adventuretype][d=difficulty]        |",
            @"|  leveling <t> <o>      | leveling the current mogwai [t=type][o=option]                             |",
            @"|  shave sheep           | test combat with a common animal                                           |",
            @"|  evolve <t>            | ... TODO [t=type]                                                          |",
            @"|  loot <o>              | ... TODO [o=option]                                                        |",
            @"|  heatndwater <t>       | ... TODO [t=time]                                                          |",
            @"+-----------------------------------------------------------------------------------------------------+",
        };

        private static readonly Lazy<CommandLine> _lazyInstance = new Lazy<CommandLine>(() => new CommandLine());

        private CachingService CachingService => Blockchain.Instance.CachingService;

        private static bool InGameMessageVerbose = true;

        public Mogwai currentMogwai;

        public static CommandLine Instance => _lazyInstance.Value;

        private CommandLine() { }

        public void Start()
        {
            XmlConfigurator.Configure();

            try
            {
                Loop();
            }
            catch (Exception e)
            {
                ConsoleError($"Terminating commandLine, with exception '{e.Message}'! check log for full exception.");
                ConsoleWarn(e.ToString());
                _log.Error(e.ToString());
                Console.ReadKey();
            }

        }

        public void Loop()
        {
            foreach (string str in logo_ascii)
            {
                ColorWriteLine(str, ConsoleColor.Cyan);
            }
            foreach (string str in help_info)
            {
                ColorWriteLine(str, ConsoleColor.DarkCyan);
            }

            // initial caching of the blockchain stuff
            Console.WriteLine("Initial caching of blockhain.");
            Blockchain.Instance.Cache(false, true);

            // put code above while loop that only needs to be executed once
            while (true)
            {
                // get the user input for every iteration, allowing to exit at will
                Console.Write((currentMogwai != null ? currentMogwai.Name : "") + bash);
                String line = Console.ReadLine().Trim();

                if (line.Equals("help"))
                {
                    string[] help = currentMogwai == null ? help_info : mogwai_control;
                    foreach (string str in help)
                    {
                        ColorWriteLine(str, ConsoleColor.DarkCyan);
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
                else if (line.StartsWith("show"))
                {
                    Show(line);
                }
                else if (line.StartsWith("cache"))
                {
                    Cache(line);
                }
                else if (line.Equals("exit"))
                {
                    Blockchain.Instance.Exit();
                    return;
                }
                // commands without a current mogwai
                else if (currentMogwai == null)
                {
                    if (line.StartsWith("mogwais"))
                    {
                        Mogwais(line);
                    }
                    else if (line.Equals("create"))
                    {
                        Create();
                    }
                    else if (line.StartsWith("bind"))
                    {
                        Bind(line);
                    }

                    else if (line.StartsWith("choose"))
                    {
                        Choose(line);
                    }
                }
                // commands that are only valid wit a current mogwai
                else if (currentMogwai != null && line.Equals("dismiss"))
                {
                    ConsoleResponse($"You've dismissed {currentMogwai.Name}!");
                    currentMogwai = null;
                }
                else if (currentMogwai != null && line.StartsWith("update"))
                {
                    Update(line);
                }
                else if (currentMogwai != null && line.StartsWith("shave sheep"))
                {
                    ShaveSheep();
                }
                else if (currentMogwai != null && line.StartsWith("adventure"))
                {
                    Adventure(line);
                }
                else if (currentMogwai != null && line.StartsWith("leveling"))
                {
                    Leveling(line);
                }
                else
                {
                    ConsoleWarn($"Command '{line}' isn't supported, please check 'help' for further guidance.");
                }
                // this is what will happen in the loop body since we didn't exit
                // put whatever note stuff you want to execute again and again in here
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        private void Adventure(string line)
        {
            string[] strArray = line.Split(' ');
            if (strArray.Count() == 3 && Enum.TryParse(strArray[1], out AdventureType adventureType)
                && Enum.TryParse<DifficultyType>(strArray[2], out DifficultyType difficultyType)
                && Blockchain.Instance.SendInteraction(currentMogwai.Key, new AdventureAction(adventureType, difficultyType, currentMogwai.CurrentLevel)))
            {
                ConsoleResponse($"Successfuly, sent adventure action to the mogwai.");

            }
            else
            {
                ConsoleWarn($"Wrong number of arguments, check help for detailed informations!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        private void Leveling(string line)
        {
            string[] strArray = line.Split(' ');
            if (strArray.Count() == 3 && Enum.TryParse(strArray[1], out LevelingType levelingType))
            {
                if (Enum.TryParse<ClassType>(strArray[2], out ClassType classType)
                && Blockchain.Instance.SendInteraction(currentMogwai.Key, new LevelingAction(levelingType, classType, currentMogwai.CurrentLevel, currentMogwai.GetClassLevel(classType))))
                {
                    ConsoleResponse($"Successfuly, sent leveling action to the mogwai.");
                }
                else
                {
                    ConsoleWarn($"Unknown selection action!");
                }
            }
            else
            {
                ConsoleWarn($"Wrong number of arguments, check help for detailed informations!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        private void Update(string line)
        {
            string[] strArray = line.Split(' ');
            if (strArray.Count() == 1 && strArray[0].Equals("update"))
            {
                int oldPointer = currentMogwai.Pointer;
                currentMogwai.Evolve();
                CachingService.MogwaisCache.Update(currentMogwai);
                CachingService.Persist(false, true);
            }
            else if (strArray.Count() == 2 && strArray[1].Equals("restart"))
            {
                if (Blockchain.Instance.TryGetMogwai(currentMogwai.Key, false, out Mogwai mogwai) == Blockchain.BoundState.BOUND)
                {
                    ConsoleResponse($"Pointer reseted for {mogwai.Name} [{mogwai.CurrentLevel}]! Pointer: {mogwai.Pointer}");
                    currentMogwai = mogwai;
                }
            }
            else if (strArray.Count() == 2 && strArray[1].Equals("shift"))
            {

            }
            else
            {
                ConsoleWarn($"Wrong number of arguments, check help for detailed informations!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Create()
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        private void Bind(string line)
        {
            string[] strArray = line.Split(' ');
            if (strArray.Count() == 2 && strArray[1].StartsWith("M") && strArray[1].Length == 34)
            {
                if (Blockchain.Instance.BindMogwai(strArray[1]))
                {
                    ConsoleResponse($"Successfuly, bound a mogwai, please wait till the connection is established.");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        private void Show(string line)
        {
            string[] strArray = line.Split(' ');
            if (strArray.Count() == 1 && strArray[0].Equals("show"))
            {
                if (currentMogwai != null)
                {
                    Print(currentMogwai);
                }
                else
                {
                    ConsoleWarn($"No mogwai choosen to show!");
                }
            }
            else if (strArray.Count() == 2 && strArray[1].StartsWith("M") && strArray[1].Length == 34)
            {
                if (Blockchain.Instance.TryGetMogwai(strArray[1], false, out Mogwai mogwai) == Blockchain.BoundState.BOUND)
                {
                    Print(mogwai);
                }
                else
                {
                    ConsoleWarn($"Couldn't show mogwai!");
                }
            }
            else if (strArray.Count() == 2 && strArray[1].Equals("all"))
            {
                var mogwaiAddressesDict = Blockchain.Instance.ValidMogwaiAddresses();
                foreach (var keyValue in mogwaiAddressesDict)
                {
                    if (Blockchain.Instance.TryGetMogwai(keyValue.Key, false, out Mogwai mogwai) == Blockchain.BoundState.BOUND)
                    {
                        Print(mogwai);
                    }
                }
            }
            else
            {
                ConsoleWarn($"Wrong number of arguments, check help for detailed informations!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        private void Cache(string line)
        {
            string[] strArray = line.Split(' ');
            if (strArray.Count() == 1)
            {
                Blockchain.Instance.Cache(false, true);
            }
            else if (strArray.Count() == 2)
            {
                switch (strArray[1])
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

        /// <summary>
        /// 
        /// </summary>
        private void Mogwais(string line)
        {
            string[] strArray = line.Split(' ');
            BoundState boundState = BoundState.BOUND;
            if (strArray.Count() == 2 && Enum.TryParse<BoundState>(strArray[1], out boundState))
            {

            }

            var mogwaiAddressesDict = Blockchain.Instance.ValidMogwaiAddresses();
            string defaultStr = "+------------------------------------+-------+-------+------------+";
            ConsoleResponse(defaultStr);
            ConsoleResponse($"+ Address                            + Bound + Level + Funds      |");
            ConsoleResponse(defaultStr);
            foreach (var keyValue in mogwaiAddressesDict)
            {
                var key = keyValue.Key;
                var unspent = Blockchain.Instance.UnspendFunds(key, out List<ListUnspentResponse> listUnspent);
                var created = Blockchain.Instance.TryGetMogwai(key, false, out Mogwai mogwai);
                if (strArray.Count() == 1 || created == boundState)
                {
                    ConsoleResponse($"| {string.Format("{0,34}", key)} | {string.Format("{0,5}", created)} | {string.Format("{0,5}", mogwai == null ? 0 : mogwai.CurrentLevel)} | {string.Format("{0:###0.0000}", unspent).PadLeft(10).Substring(0, 10)} |");
                    ConsoleResponse(defaultStr);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShaveSheep()
        {
            ConsoleResponse("Let's start and shave a sheep!");
            Shift shift = new Shift(0D,
                1531171420,
                "32f13027e869de56de3c2d5af13f572b67b5e75a18594013ec",
                9196,
                "000000001f2ade78b094fce0fbfacc55da3a23ec82489171eb2687a1b6582d13",
                11,
                "9679a3d39efdf8faa019410250fa91647a76cbb1bd2fd1c5d7ba80551b4edd7b",
                1.00m,
                0.0001m);
            SimpleCombat combat = new SimpleCombat(new List<Monster>() { Monsters.Rat });
            combat.Create(currentMogwai, shift);
            combat.Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        private void Choose(string line)
        {
            string[] strArray = line.Split(' ');
            if (strArray.Count() == 2 && strArray[1].StartsWith("M") && strArray[1].Length == 34)
            {
                if (Blockchain.Instance.TryGetMogwai(strArray[1], false, out Mogwai mogwai) == Blockchain.BoundState.BOUND)
                {
                    foreach (string str in mogwai_control)
                    {
                        ColorWriteLine(str, ConsoleColor.DarkCyan);
                    }
                    currentMogwai = mogwai;
                    InGameMessageVerbose = false;
                    mogwai.Evolve(CachingService.MogwaisCache.Pointer(currentMogwai));
                    InGameMessageVerbose = true;
                    ConsoleResponse($"You've choosen {mogwai.Name} [{mogwai.CurrentLevel}]! pointer: {mogwai.Pointer}");
                }
                else
                {
                    ConsoleWarn($"Couldn't choose mogwai!");
                }
            }
            else
            {
                ConsoleWarn($"Wrong number of arguments or invalid mogwaiaddress, check help for detailed informations!");
            }
        }

        private void Print(Mogwai mogwai)
        {
            string[] template1 = new string[] {
            @"  .+------------------------------+.",
            @".:-| Name: <name   > [Lvl. <lev>] |-:. EXP: <       exp>".Replace("<name   >", string.Format("{0}", mogwai.Name).PadLeft(9)).Replace("<lev>", string.Format("{0:####0}", mogwai.CurrentLevel).PadLeft(5)).Replace("<       exp>", string.Format("{0}", mogwai.Exp).PadLeft(12)),
            @"+-stats----------+-------------------+-body--------------+",
            @"¦ STR:   <str  > ¦ GENDER: <gen    > ¦ SKINCOL:  <skinc> ¦".Replace("<str  >", string.Format("{0,7}",mogwai.Strength)).Replace("<gen    >", string.Format("{0,9}",mogwai.MapGender)).Replace("<skinc>", string.Format("{0,7}",mogwai.Body.SkinColor)),
            @"¦ CON:   <con  > +-state-------------+ EYECOL:   <eyec > |".Replace("<con  >", string.Format("{0,7}",mogwai.Constitution)).Replace("<eyec >", string.Format("{0,7}",mogwai.Body.EyeColor)),
            @"¦ DEX:   <dex  > ¦ HP:   <hp       > ¦ EAR:      <ear  > ¦".Replace("<dex  >", string.Format("{0,7}",mogwai.Dexterity)).Replace("<hp       >", string.Format("{0,11}",mogwai.CurrentHitPoints)).Replace("<ear  >", string.Format("{0,7}",mogwai.Body.Ear)),
            @"¦ INT:   <int  > ¦ MANA: <mana     > ¦ MOUTH:    <mouth> ¦".Replace("<int  >", string.Format("{0,7}",mogwai.Inteligence)).Replace("<mouth>", string.Format("{0,7}",mogwai.Body.Mouth)),
            @"¦ WIS:   <wis  > ¦ END:  <end      > +-coat--------------+".Replace("<wis  >", string.Format("{0,7}",mogwai.Wisdom)),
            @"¦ CHA:   <cha  > +-------------------+ COATTYPE: <ctype> ¦".Replace("<cha  >", string.Format("{0,7}",mogwai.Charisma)).Replace("<ctype>", string.Format("{0,7}",mogwai.Coat.CoatType)),
            @"¦ LUC:   <luc  > ¦ ALLI: <alli     > ¦ COATGEN:  <cgen > ¦".Replace("<luc  >", string.Format("{0,7}",mogwai.Stats.Luck)).Replace("<alli     >", string.Format("{0,11}",mogwai.Stats.MapAllignment())).Replace("<cgen >", string.Format("{0,7}",mogwai.Coat.CoatGenetic)),
            @"+----------------+-------------------+ COLOR1:   <col1 > ¦".Replace("<col1 >", string.Format("{0,7}",mogwai.Coat.CoatColor1)),
            @"¦ <address                         > | COLOR2:   <col2 > ¦".Replace("<address                         >", string.Format("{0,34}",mogwai.Key)).Replace("<col2 >", string.Format("{0,7}",mogwai.Coat.CoatColor2)),
            @"+------------------------------------+-------------------+" };

            string[] template = new string[]
            {
                @"  .+------------------------------+.                         ¬", // 0
                @".:-¦ <name        >    [Lvl. <l>] ¦-:. Class: <class       > ¬", // 1
                @"+-stats-------+-hitpoints---------+--+-experience-----------+¬", // 2
                @"¦ STR: <str > ¦ <chp > / <mhp > HP   ¦ <cex > / <nex > EXP  ¦¬", // 3
                @"¦ DEX: <dex > ¦ <hpbar        > <h%> ¦ <exbar        > <e%> ¦¬", // 4
                @"¦ CON: <con > +-combat------+-weapon-+-----+---+------------+¬", // 5
                @"¦ INT: <int > ¦ INI: <ini > ¦ P: <pweap      > dmg <pdmg  > ¦¬", // 6
                @"¦ WIS: <wis > ¦ AT:  <atb > ¦ S: <sweap      > dmg <sdmg  > ¦¬", // 7
                @"¦ CHA: <cha > ¦ AC:  <acl > +-armor-------------------------+¬", // 8
                @"+-------------+ SP:  <sp  > ¦ <barm          >  AC:  <baac> ¦¬", // 9
                @"¦ FOR: <for > +-------------+ ................  AC:  <... > ¦¬", //10
                @"¦ REF: <ref > ¦ CMB: <cmb > ¦ ................  AC:  <... > ¦¬", //11
                @"¦ WIL: <wil > ¦ CMD: <cmd > ¦ ................  AC:  <... > ¦¬", //12
                @"+-------------+-------------+-------------------------------+¬", //13
                @"¦ <address                         > ¦ <alligen           > ¦¬", //14
                @"+-------------+-------------+-------------------------------+¬"  //15
            };
            template[1] = template[1].Replace("<name        >", string.Format("¬C{0}§", mogwai.Name.PadRight(14)));
            template[1] = template[1].Replace("<class       >", string.Format("¬c{0}§", (mogwai.CurrentClass != null ? mogwai.CurrentClass.Name : "none").PadRight(14)));
            template[1] = template[1].Replace("<l>", string.Format("¬Y{0:##0}§", mogwai.CurrentLevel.ToString().PadLeft(3)));
            template[2] = template[2].Replace("<gen    >", string.Format("¬W{0}§", mogwai.MapGender.PadLeft(9)));
            template[3] = template[3].Replace("<str >", string.Format("¬W{0,2}§[¬{1}{2,2}§]", mogwai.Strength, mogwai.StrengthMod > 0 ? "G" : mogwai.StrengthMod < 0 ? "R" : "Y", mogwai.StrengthMod.ToString("+0;-#")));

            template[3] = template[3].Replace("<chp >", string.Format("¬W{0}§", mogwai.CurrentHitPoints.ToString().PadLeft(6)));
            template[3] = template[3].Replace("<mhp >", string.Format("¬W{0}§", mogwai.MaxHitPoints.ToString().PadRight(6)));

            template[4] = template[4].Replace("<hpbar        >", string.Format("{0}", StringHelpers.GetBar(mogwai.CurrentHitPoints, mogwai.MaxHitPoints, out int hpPerc)));
            template[4] = template[4].Replace("<h%>", string.Format("¬W{0:##0}§%", hpPerc.ToString().PadLeft(3)));

            template[3] = template[3].Replace("<cex >", string.Format("¬W{0}§", mogwai.Exp.ToString().PadLeft(6)));
            template[3] = template[3].Replace("<nex >", string.Format("¬W{0}§", mogwai.XpToLevelUp.ToString().PadRight(6)));

            template[4] = template[4].Replace("<exbar        >", string.Format("{0}", StringHelpers.GetBar(mogwai.Exp, mogwai.XpToLevelUp, out int exPerc)));
            template[4] = template[4].Replace("<e%>", string.Format("¬W{0:##0}§%", exPerc.ToString().PadLeft(3)));

            template[4] = template[4].Replace("<dex >", string.Format("¬W{0,2}§[¬{1}{2,2}§]", mogwai.Dexterity, mogwai.DexterityMod > 0 ? "G" : mogwai.DexterityMod < 0 ? "R" : "Y", mogwai.DexterityMod.ToString("+0;-#")));
            template[5] = template[5].Replace("<con >", string.Format("¬W{0,2}§[¬{1}{2,2}§]", mogwai.Constitution, mogwai.ConstitutionMod > 0 ? "G" : mogwai.ConstitutionMod < 0 ? "R" : "Y", mogwai.ConstitutionMod.ToString("+0;-#")));
            template[6] = template[6].Replace("<int >", string.Format("¬W{0,2}§[¬{1}{2,2}§]", mogwai.Inteligence, mogwai.InteligenceMod > 0 ? "G" : mogwai.InteligenceMod < 0 ? "R" : "Y", mogwai.InteligenceMod.ToString("+0;-#")));
            template[7] = template[7].Replace("<wis >", string.Format("¬W{0,2}§[¬{1}{2,2}§]", mogwai.Wisdom, mogwai.WisdomMod > 0 ? "G" : mogwai.WisdomMod < 0 ? "R" : "Y", mogwai.WisdomMod.ToString("+0;-#")));
            template[8] = template[8].Replace("<cha >", string.Format("¬W{0,2}§[¬{1}{2,2}§]", mogwai.Charisma, mogwai.CharismaMod > 0 ? "G" : mogwai.CharismaMod < 0 ? "R" : "Y", mogwai.CharismaMod.ToString("+0;-#")));

            template[6] = template[6].Replace("<ini >", string.Format("¬W{0}§", mogwai.Initiative.ToString().PadLeft(2).PadRight(6)));
            int addAtb = mogwai.AttackBonus(0) - mogwai.BaseAttackBonus[0];
            template[7] = template[7].Replace("<atb >", string.Format("¬W{0,2}§[¬{1}{2,2}§]", mogwai.BaseAttackBonus[0], addAtb > 0 ? "G" : addAtb < 0 ? "R" : "Y", addAtb.ToString("+0;-#")));
            template[8] = template[8].Replace("<acl >", string.Format("¬W{0}§", mogwai.ArmorClass.ToString().PadLeft(2).PadRight(6)));

            template[6] = template[6].Replace("<pweap      >", string.Format("¬W{0}§", mogwai.Equipment.PrimaryWeapon.Name.PadRight(13)));
            template[6] = template[6].Replace("<pdmg  >", "¬Y" + string.Format("{0}-{1}", mogwai.Equipment.PrimaryWeapon.MinDmg, mogwai.Equipment.PrimaryWeapon.MaxDmg).PadRight(8) + "§");
            if (mogwai.Equipment.SecondaryWeapon != null)
            {
                template[7] = template[7].Replace("<sweap      >", string.Format("¬W{0}§", mogwai.Equipment.SecondaryWeapon.Name.PadRight(13)));
                template[7] = template[7].Replace("<sdmg  >", "¬Y" + string.Format("{0}-{1}", mogwai.Equipment.SecondaryWeapon.MinDmg, mogwai.Equipment.SecondaryWeapon.MaxDmg).PadRight(8) + "§");
            }
            else
            {
                template[7] = template[7].Replace("<sweap      >", "none         ");
                template[7] = template[7].Replace("<sdmg  >", "        ");
            }

            if (mogwai.Equipment.Armor != null)
            {
                template[9] = template[9].Replace("<barm          >", string.Format("¬W{0}§", mogwai.Equipment.Armor.Name.PadRight(16)));
                template[9] = template[9].Replace("<baac>", "¬Y" + string.Format("{0}", mogwai.Equipment.Armor.ArmorBonus).PadRight(6) + "§");
            }
            else
            {
                template[9] = template[9].Replace("<barm          >", "none            ");
                template[9] = template[9].Replace("<sdmg  >", "        ");
            }

            template[14] = template[14].Replace("<address                         >", "¬G" + string.Format("{0,34}", mogwai.Key) + "§");
            template[14] = template[14].Replace("<alligen           >", "¬W" + string.Format("{0} {1}", mogwai.Stats.MapAllignment(), mogwai.MapGender).PadRight(20) + "§");



            foreach (string str in template)
            {
                StringHelpers.Msg(str);
            }

            // debug
            //mogwai.Print();
        }

        public void ConsoleWarn(string value)
        {
            ColorWriteLine(value, ConsoleColor.Yellow);
        }

        public void ConsoleResponse(string value)
        {
            ColorWriteLine(value, ConsoleColor.Green);
        }

        public void ConsoleError(string value)
        {
            ColorWriteLine(value, ConsoleColor.Red);
        }

        public static void ColorWriteLine(string value, ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
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

        public static void InGameMessage(string value, bool line = false)
        {
            InGameMessage(value, ConsoleColor.White, ConsoleColor.Black, line);
        }
        public static void InGameMessage(string value, ConsoleColor foreground, bool line = false)
        {
            InGameMessage(value, foreground, ConsoleColor.Black, line);
        }
        public static void InGameMessage(string value, ConsoleColor foreground, ConsoleColor background, bool line)
        {
            if (!InGameMessageVerbose)
            {
                return;
            }

            //
            // This method writes an entire line to the console with the string.
            //
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
            Console.Write(value.PadRight(value.Length - 1)); // <-- see note
            if (line)
            {
                Console.WriteLine();
            }
            //
            // Reset the color.
            //
            Console.ResetColor();
        }

    }
}
