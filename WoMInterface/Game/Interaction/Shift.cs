using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game.Enums;
using WoMInterface.Game.Random;

namespace WoMInterface.Game.Interaction
{

    public class Shift
    {
        public double Index { get; }

        public double Time { get; set; }
        public string AdHex { get; set; }
        public int Height { get; set; }
        public string BkHex { get; set; }
        public double BkIndex { get; set; }
        public string TxHex { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }

        public bool IsSmallShift => TxHex == null;

        private Dice dice;
        public Dice Dice => dice ?? (dice = new Dice(this));

        public InteractionType InteractionType { get; }

        public Shift(double index)
        {
            Index = index;
            InteractionType = GetInteractionType();
            
        }

        private InteractionType GetInteractionType()
        {
            if (Index == 0)
            {
                return InteractionType.CREATION;
            }

            if (IsSmallShift)
            {
                return InteractionType.NONE;
            }

            decimal rAmount = Amount - Fee;
            string parm1 = (Amount - Fee).ToString("0.00000000").Split('.')[1];
            string saveParam = Fee.ToString("0.00000000").Split('.')[1].Substring(5);
            string costType = parm1.Substring(0, 2);
            string actionType = parm1.Substring(2, 2);
            string addParam = parm1.Substring(4, 4);

            //  action part 1
            //    ct at spec           
            //  0.00 00 0000

            //  action part 2
            //         save
            //  0.0001 0000

            return InteractionType.UNDEFINED;

        }

        public override string ToString()
        {
            return $"{Time};{AdHex};{Height};{BkHex};{BkIndex};{TxHex};{Amount}";
        }

        public void Action()
        {

        }
    }

}
