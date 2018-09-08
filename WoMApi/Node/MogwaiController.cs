using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMApi.Node
{
    public class MogwaiController
    {
        public MogwaiWallet Wallet { get; }

        public MogwaiController()
        {
            Wallet = new MogwaiWallet();
        }
    }
}
