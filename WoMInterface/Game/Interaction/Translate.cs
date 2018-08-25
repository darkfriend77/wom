using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game.Enums;

namespace WoMInterface.Game.Interaction
{
    class Translate
    {
        public static decimal[] GetAmountAndFee(Interaction interaction)
        {
            decimal amount = 0.0m;
            decimal fee = 0.0m;




            return new decimal[] { amount, fee };
        }

        public static Interaction GetInteraction(decimal amount, decimal fee)
        {
            InteractionType interactionType = InteractionType.NONE;


            return null;
        }
    }
}
