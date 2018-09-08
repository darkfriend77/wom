using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Surfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMApi.Node;

namespace SadMogwai.Consoles
{
    class WalletConsole : SadConsole.Console
    {
        private MogwaiWallet wallet;
        private Basic borderSurface;
        private int counter = 0;
        public WalletConsole(MogwaiController controller, int width, int height) : base(width, height)
        {
            borderSurface = new Basic(width + 2, height + 2, base.Font);
            borderSurface.DrawBox(new Rectangle(0, 0, borderSurface.Width, borderSurface.Height),
                                  new Cell(Color.DarkCyan, Color.Black), null, SurfaceBase.ConnectedLineThick);
            borderSurface.Position = new Point(-1, -1);
            Children.Add(borderSurface);
            wallet = controller.Wallet;
            //UpdatedWallet();
        }

        public void UpdatedWallet()
        {
            //Print(1, 0, "Mogwais:", Color.DarkCyan);
            //if (wallet.MogwaiKeyDict != null)
            //{
            //    Print(27, 0, $"{wallet.MogwaisBound.ToString()} / {wallet.MogwaiAddresses.ToString()}", Color.Orange);
            //}
            //Print(40, 0, "Funds:", Color.DarkCyan);
            //if (wallet.Deposit != null)
            //{
            //    Print(74, 0, $"[c:g f:LimeGreen:Orange:34]{wallet.Deposit.Address}");
            //}
            //Print(65, 0, $"DEPOSIT:", Color.DarkCyan);
            //if (wallet.Deposit != null)
            //{
            //    Print(74, 0, $"[c:g f:LimeGreen:Orange:34]{wallet.Deposit.Address}");
            //}
        }

        public override void Update(TimeSpan delta)
        {
            base.Update(delta);
            if (counter++ % 200 == 0)
            {
                //Print(1, 0, counter.ToString(), Color.Orange);
                UpdatedWallet();
                counter = 0;
            }
        }
    }
}
