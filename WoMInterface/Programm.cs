using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game.Model;
using WoMInterface.Node;
using WoMInterface.Tool;

namespace WoMInterface
{
    class Programm
    {
        static void Main(string[] args)
        {

            var instance = new Blockchain("http://127.0.0.1:17710", "mogwai","mogwai","");
            instance.TryGetMogwai("MJHYMxu2kyR1Bi4pYwktbeCM7yjZyVxt2i", out Mogwai mogwai);

            Console.WriteLine(mogwai.Name);
            while(mogwai.Evolve(out GameLog history))
            {
                if (history != null) {
                    history.logEntries.ForEach(p => Console.WriteLine(p.ToString()));
                }
            }
            Console.ReadKey();

            File.WriteAllText("mogwai.json", JsonConvert.SerializeObject(mogwai.Shifts));


        }
    }
}
