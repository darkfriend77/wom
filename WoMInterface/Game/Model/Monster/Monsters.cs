using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game.Enums;
using WoMInterface.Game.Random;

namespace WoMInterface.Game.Model
{
    public partial class Monsters
    {

        public static List<Monster> Animals { get; set; }

        public Monsters()
        {
            Animals = new List<Monster>()
            {
                Rat,
                Wolf
            };
        }
    }
}
