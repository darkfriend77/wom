using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMInterface.Game
{
    public class Shift
    {
        public double Time { get; set; }
        public string AdHex { get; set; }
        public int Height { get; set; }
        public string BkHex { get; set; }
        public double BkIndex { get; set; }
        public string TxHex { get; set; }
        public decimal Amount { get; set; }

        override
        public string ToString()
        {
            return $"{Time};{AdHex};{Height};{BkHex};{BkIndex};{TxHex};{Amount}";
        }
    }

    public class SmallShift : Shift
    {

    }
}
