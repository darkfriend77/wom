using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game.Enums;

namespace WoMInterface.Game.Model
{
    public class NoClass : Classes
    {
        public NoClass() : base(ClassType.NONE)
        {
        }

        public override void ClassLevelUp()
        {
            base.ClassLevelUp();
        }
    }
}
