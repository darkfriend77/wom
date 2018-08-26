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
using WoMInterface.Game.Enums;
using WoMInterface.Game.Interaction;
using WoMInterface.Node;
using WoMInterface.Tool;

namespace WoMInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Instance.Start();

            //Test();
        }

        static void Test()
        {
            decimal amount = 0.12345678m;
            decimal fee = 0.00011234m;
            string parm1 = (amount - fee).ToString("0.00000000").Split('.')[1];
            string saveParm = fee.ToString("0.00000000").Split('.')[1].Substring(4);
            string costType = parm1.Substring(0, 2);
            string actionType = parm1.Substring(2, 2);
            string addParm = parm1.Substring(4, 4);
            Console.WriteLine($"org. amount:  {amount}");
            Console.WriteLine($"org. fee:     {fee}");
            Console.WriteLine($"# data #############");
            Console.WriteLine($"- costType:   {costType}");
            Console.WriteLine($"- actionType: {actionType}");
            Console.WriteLine($"- addParm:    {addParm}");
            Console.WriteLine($"- saveParm:   {saveParm}");

            Adventure adventure = new Adventure(AdventureType.CHAMBER, DifficultyType.CHALLENGING, 2);
            Console.WriteLine($"Value1: {adventure.GetValue1()}");
            Console.WriteLine($"Value2: {adventure.GetValue2()}");

            Console.ReadKey();
        }
    }
}
