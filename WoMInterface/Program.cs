using BitcoinLib.Services.Coins.Cryptocoin;
using BitcoinLib.Services.Coins.Mogwaicoin;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game;
using WoMInterface.Node;
using WoMInterface.Tool;

namespace WoMInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Instance.Start();
        }
    }
}
