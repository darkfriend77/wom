using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game.Enums;

namespace WoMInterface.Game.Interaction
{
    class SelectionAction : Interaction
    {
        public SelectionType SelectionType { get; }

        public ClassType ClassType { get; }

        public SelectionAction(InteractionType interactionType, SelectionType selectionType, ClassType classType) : base(interactionType)
        {
            SelectionType = selectionType;
            ClassType = classType;
            ParamAdd1 = ((int)selectionType * 100) + (int)classType;
            // not really used, but can be freed later ...
            ParamAdd2 = 0;
        }
    }
}
