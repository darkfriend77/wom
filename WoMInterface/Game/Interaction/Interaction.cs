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

        public string ParamAdd1 { get; set; }

        public string ParamAdd2 { get; set; }

        public Interaction(InteractionType interactionType)
        {
            InteractionType = interactionType;

        }

    }
}
