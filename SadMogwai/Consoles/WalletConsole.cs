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
        private readonly MogwaiController controller;
        private Basic borderSurface;
        private int counter = 0;
        public WalletConsole(MogwaiController mogwaiController, int width, int height) : base(width, height)
        {
            borderSurface = new Basic(width + 2, height + 2, base.Font);
            borderSurface.DrawBox(new Rectangle(0, 0, borderSurface.Width, borderSurface.Height),
                                  new Cell(Color.DarkCyan, Color.Black), null, SurfaceBase.ConnectedLineThick);
            borderSurface.Position = new Point(-1, -1);
            Children.Add(borderSurface);
            controller = mogwaiController;
        }

        public override void Update(TimeSpan delta)
        {
            Print(1, 0, "Mogwais:", Color.DarkCyan);
            Print(49, 0, "Funds:", Color.DarkCyan);
            Print(56, 0, controller.GetDepositFunds().ToString("###0.00"), Color.Orange);
            Print(65, 0, $"Deposit:", Color.DarkCyan);
            Print(74, 0, $"[c:g f:LimeGreen:Orange:34]{controller.DepositAddress}");
            base.Update(delta);
        }
    }
}
