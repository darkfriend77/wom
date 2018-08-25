using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game.Enums;

namespace WoMInterface.Game.Interaction
{
    public abstract class Interaction
    {
        public CostType CostType { get; set; }

        public InteractionType InteractionType { get; set; }

        public int ParamAdd1 { get; set; }

        public int ParamAdd2 { get; set; }

        public Interaction(InteractionType interactionType)
        {
            CostType = CostType.STANDARD; 
            InteractionType = interactionType;
        }

        public decimal GetValue1()
        {
            int value = (int)CostType * 1000000 + (int)InteractionType * 10000 + ParamAdd1;
            return decimal.Parse("0." + value.ToString().PadLeft(8, '0'));
        }

        public decimal GetValue2()
        {
            return decimal.Parse("0." + ParamAdd2.ToString().PadLeft(8, '0'));
        }
    }
}
