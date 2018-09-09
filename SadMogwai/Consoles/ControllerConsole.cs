using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using SadConsole.Surfaces;
using SadMogwai.Art;
using System;
using System.Collections.Generic;
using System.Linq;
using WoMApi.Node;

namespace SadMogwai.Consoles
{
    class ControllerConsole : SadConsole.Console
    {
        private static SelectionScreen _selectionConsole;
        private static MogwaiConsole _welcome;
        private static WalletConsole _walletConsole;
        private static MogwaiConsole _info;
        private static MogwaiConsole _command;

        private static MogwaiController _controller;

        public bool IsActive { get; set; }

        public ControllerConsole(MogwaiController mogwaiController, int width, int height) : base(width, height)
        {
            _welcome = new MogwaiConsole("Welcome", "Mogwaicoin Team 2018", 110, 6)
            {
                Position = new Point(1, 1)
            };
            for (int i = 0; i < Ascii.Logo.Length; i++)
            {
                string str = Ascii.Logo[i];
                _welcome.Print(4, i, $"[c:g b:0,0,0:Black:DarkCyan:DarkGoldenRod:DarkRed:Black:0,0,0:{str.Length}][c:g f:LimeGreen:Orange:{str.Length}]" + str, Color.Cyan, Color.Black);
            }

            _controller = mogwaiController;

            _walletConsole = new WalletConsole(mogwaiController, 110, 2);
            _walletConsole.Position = new Point(1, 9);
            _selectionConsole = new SelectionScreen(mogwaiController, 110, 22);
            _selectionConsole.Position = new Point(1, 12);
            _info = new MogwaiConsole("Info", "", 24, 38);
            _info.Position = new Point(114, 1);
            _command = new MogwaiConsole("Command", "", 110, 3);
            _command.Position = new Point(1, 36);

            IsActive = false;

        }

        public void Show()
        {
            Children.Add(_welcome);
            Children.Add(_walletConsole);
            Children.Add(_selectionConsole);
            Children.Add(_command);
            Children.Add(_info);
            IsActive = true;
        }

        public void HIde()
        {
            Children.Clear();
            IsActive = false;
        }

        public override bool ProcessKeyboard(Keyboard state)
        {
            if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.C))
            {
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.S))
            {
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.B))
            {
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                return true;
            }

            return false;
        }

        public override void Update(TimeSpan delta)
        {
            base.Update(delta);
        }
    }


}
