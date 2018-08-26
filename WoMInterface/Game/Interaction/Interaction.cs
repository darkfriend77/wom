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

        public static Interaction GetInteraction(decimal amount, decimal fee)
        {
            string parm1 = (amount - fee).ToString("0.00000000").Split('.')[1];
            CostType costType = (CostType) int.Parse(parm1.Substring(0, 2));
            InteractionType interactionType = (InteractionType) int.Parse(parm1.Substring(2, 2));
            int addParam1 = int.Parse(parm1.Substring(4, 4));
            int addParam2 = int.Parse(fee.ToString("0.00000000").Split('.')[1].Substring(4, 4));
            switch (interactionType)
            {
                case InteractionType.NONE:
                    throw new NotImplementedException();

                case InteractionType.CREATION:
                    throw new NotImplementedException();

                case InteractionType.MODIFICATION:
                    throw new NotImplementedException();

                case InteractionType.LEVELING:
                    throw new NotImplementedException();

                case InteractionType.ADVENTURE:
                    return new Adventure(addParam1, addParam2);

                case InteractionType.DUELL:
                    throw new NotImplementedException();

                case InteractionType.BREEDING:
                    throw new NotImplementedException();

                case InteractionType.LOOTING:
                    throw new NotImplementedException();

                case InteractionType.UNDEFINED:
                    throw new NotImplementedException();

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
