using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using SadConsole.Surfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using WoMApi.Node;

namespace SadMogwai.Consoles
{
    public class SelectionScreen : SadConsole.Console
    {
        private int oldPointer, pointer;

        private Color rowNormal = Color.DarkCyan;

        private Color highNormal = Color.Cyan;

        private Basic borderSurface;

        private int glyphIndex = 185;

        private MogwaiController controller;

        private List<MogwaiKeys> MogwaiKeysList => controller.MogwaiKeysDict.Values.ToList();

        public int headerPosition = 3;

        public bool IsReady { get; set; } = false;

        public SelectionScreen(MogwaiController mogwaiController, int width, int height) : base(width, height)
        {
            borderSurface = new Basic(width + 2, height + 2, base.Font);
            borderSurface.DrawBox(new Rectangle(0, 0, borderSurface.Width, borderSurface.Height),
                                  new Cell(Color.DarkCyan, Color.Black), null, SurfaceBase.ConnectedLineThick);
            borderSurface.Position = new Point(-1, -1);
            Children.Add(borderSurface);

            CreateHeader();

            controller = mogwaiController;
            pointer = 0;
            oldPointer = 0;
        }

        public void Init()
        {
            IsReady = true;
            controller.Refresh(10);
        }

        private void CreateHeader()
        {
            borderSurface.Print(1, headerPosition, "[c:sg 205:110]".PadRight(124), Color.DarkCyan);
            borderSurface.SetGlyph(0, headerPosition, 204, Color.DarkCyan);
            borderSurface.SetGlyph(111, headerPosition, 185, Color.DarkCyan);
            borderSurface.SetGlyph(03, headerPosition, 185, Color.DarkCyan);
            borderSurface.Print(04, headerPosition, " Address ");
            borderSurface.SetGlyph(39, headerPosition, 185, Color.DarkCyan);
            borderSurface.Print(40, headerPosition, " State ");
            borderSurface.SetGlyph(48, headerPosition, 185, Color.DarkCyan);
            borderSurface.Print(49, headerPosition, " Funds ");
            borderSurface.SetGlyph(60, headerPosition, 185, Color.DarkCyan);
            borderSurface.Print(61, headerPosition, " Name ");
            borderSurface.SetGlyph(74, headerPosition, 185, Color.DarkCyan);
            borderSurface.Print(75, headerPosition, " Rating ");
            borderSurface.SetGlyph(84, headerPosition, 185, Color.DarkCyan);
            borderSurface.Print(85, headerPosition, " Level ");
            borderSurface.SetGlyph(93, headerPosition, 185, Color.DarkCyan);
            borderSurface.Print(94, headerPosition, " Gold ");
        }

        public override bool ProcessKeyboard(Keyboard state)
        {
            if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.C))
            {
                controller.NewMogwaiKeys();
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.S))
            {
                controller.SendMog(MogwaiKeysList[pointer]);
                Print(1, 20, "Sending mogs!", Color.Orange);
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.B))
            {
                controller.BindMogwai(MogwaiKeysList[pointer]);
                Print(1, 20, "Binding Mogwai!", Color.Orange);
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.P))
            {
                controller.PrintMogwaiKeys();
                Print(1, 20, "Print MogwaiKeys!", Color.Orange);
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                if (pointer < controller.MogwaiKeysDict.Count() - 1)
                {
                    oldPointer = pointer;
                    pointer++;
                }
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                if (pointer > 0)
                {
                    oldPointer = pointer;
                    pointer--;
                }
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                borderSurface.SetGlyph(0, 0, ++glyphIndex, Color.DarkCyan);
                borderSurface.Print(10, 0, glyphIndex.ToString(), Color.Yellow);
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                borderSurface.SetGlyph(0, 0, --glyphIndex, Color.DarkCyan);
                borderSurface.Print(10, 0, glyphIndex.ToString(), Color.Yellow);
                return true;
            }

            return false;
        }

        public override void Update(TimeSpan delta)
        {
            if (IsReady)
            {
                Print(1, 0, "Mogwais:", Color.DarkCyan);
                Print(49, 0, "Funds:", Color.DarkCyan);
                Print(56, 0, controller.GetDepositFunds().ToString("###0.00"), Color.Orange);
                Print(65, 0, $"Deposit:", Color.DarkCyan);
                Print(74, 0, $"[c:g f:LimeGreen:Orange:34]{controller.DepositAddress}");


                var list = MogwaiKeysList;
                for (int i = 0; i < list.Count; i++)
                {
                    PrintRow(i + headerPosition, list[i]);
                }
                PrintRow(pointer + headerPosition, list[pointer], true);
            }
            base.Update(delta);
        }

        private void PrintRow(int index, MogwaiKeys mogwaiKeys, bool selected = false)
        {


            var str = mogwaiKeys.Address.PadRight(36)
            + mogwaiKeys.Balance.ToString("####0.00").PadRight(10)
            + mogwaiKeys.Shifts?.Count.ToString().PadRight(10)
            + mogwaiKeys.Mogwai?.Name.ToString().PadRight(10);
            Print(1, index, str, selected ? Color.Cyan : Color.DarkCyan);
        }
    }
}
