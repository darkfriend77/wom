using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Effects;
using SadConsole.Input;
using SadConsole.Surfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using WoMWallet.Node;

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

        private ControlsConsole controlsConsole;
        private MogwaiConsole infoConsole;
        private MogwaiConsole logConsole;

        public int headerPosition;
        public int trailerPosition;

        public bool IsReady { get; set; } = false;

        public SelectionScreen(MogwaiController mogwaiController, int width, int height) : base(width, height)
        {
            borderSurface = new Basic(width + 2, height + 2, base.Font);
            borderSurface.DrawBox(new Rectangle(0, 0, borderSurface.Width, borderSurface.Height),
                                  new Cell(Color.DarkCyan, Color.Black), null, SurfaceBase.ConnectedLineThick);
            borderSurface.Position = new Point(-1, -1);
            Children.Add(borderSurface);

            controlsConsole = new ControlsConsole(110, 1);
            controlsConsole.Position = new Point(0, 24);
            controlsConsole.Fill(Color.DarkCyan, Color.Black, null);
            Children.Add(controlsConsole);

            infoConsole = new MogwaiConsole("Info", "", 24, 38);
            infoConsole.Position = new Point(113, -8);
            Children.Add(infoConsole);

            logConsole = new MogwaiConsole("Log", "", 110, 3);
            logConsole.Position = new Point(0, 27);
            Children.Add(logConsole);

            headerPosition = 1;
            trailerPosition = height - 2;

            CreateHeader();
            CreateTrailer();

            controller = mogwaiController;
            pointer = 0;
            oldPointer = 0;

        }

        public void Init()
        {
            IsReady = true;
            Print(65, 0, $"Deposit:", Color.DarkCyan);
            Print(74, 0, $"[c:g f:LimeGreen:Orange:34]{controller.DepositAddress}");
            controller.Refresh(1);
        }

        private void CreateHeader()
        {
            Print(0, headerPosition, "[c:sg 205:110]".PadRight(124), Color.DarkCyan);
            borderSurface.SetGlyph(0, headerPosition + 1, 204, Color.DarkCyan);
            borderSurface.SetGlyph(111, headerPosition + 1, 185, Color.DarkCyan);
            //SetGlyph(03, headerPosition, 185, Color.DarkCyan);

            Print(03, headerPosition, " Address ");
            SetGlyph(39, headerPosition, 185, Color.DarkCyan);
            Print(40, headerPosition, " State ");
            SetGlyph(48, headerPosition, 185, Color.DarkCyan);
            Print(49, headerPosition, " Funds ");
            SetGlyph(60, headerPosition, 185, Color.DarkCyan);
            Print(61, headerPosition, " Name ");
            SetGlyph(74, headerPosition, 185, Color.DarkCyan);
            Print(75, headerPosition, " Rating ");
            SetGlyph(84, headerPosition, 185, Color.DarkCyan);
            Print(85, headerPosition, " Level ");
            SetGlyph(93, headerPosition, 185, Color.DarkCyan);
            Print(94, headerPosition, " Gold ");
        }

        private void AddButton(int index, string text, Action<string> buttonClicked)
        {
            SetGlyph(10 + (index * 11), trailerPosition, 203, Color.DarkCyan);
            controlsConsole.SetGlyph(10 + (index * 11), 0, 186, Color.DarkCyan);
            borderSurface.SetGlyph(11 + (index * 11), trailerPosition + 3, 202, Color.DarkCyan);
            var txt = text;
            var button = new MogwaiButton(8, 1);
            button.Position = new Point(1 + (index * 11), 0);
            button.Text = txt;
            button.Click += (btn, args) =>
            {
                buttonClicked(((Button)btn).Text);
            };
            controlsConsole.Add(button);
        }

        private void CreateTrailer()
        {
            // 202 203 1086
            Print(0, trailerPosition, "[c:sg 205:110]".PadRight(124), Color.DarkCyan);
            borderSurface.SetGlyph(0, trailerPosition + 1, 204, Color.DarkCyan);
            borderSurface.SetGlyph(111, trailerPosition + 1, 185, Color.DarkCyan);

            AddButton(0, "create", DoAction);
            AddButton(1, "send", DoAction);
            AddButton(2, "bind", DoAction);
            AddButton(3, "show", DoAction);
            AddButton(4, "play", DoAction);
        }

        private void DoAction(string actionStr)
        {
            switch(actionStr)
            {
                case "create":
                    controller.NewMogwaiKeys();
                    LogInConsole("TASK", "created new mogwaikeys.");
                    break;
                case "send":
                    if (controller.HasMogwayKeys)
                    {
                        if (controller.SendMog(MogwaiKeysList[pointer]))
                        {
                            LogInConsole("DONE", $"sending mogs to address {MogwaiKeysList[pointer].Address}.");
                        }
                        else
                        {
                             LogInConsole("FAIL", $"couldn't send mogs, see log file for more information.");
                        }                        
                    }
                    break;
                case "bind":
                    if (controller.HasMogwayKeys)
                    {
                        if (controller.BindMogwai(MogwaiKeysList[pointer]))
                        {
                            LogInConsole("DONE", $"binding mogwai on address {MogwaiKeysList[pointer].Address}.");
                        }
                        else
                        {
                             LogInConsole("FAIL", $"couldn't bind mogwai, see log file for more information.");
                        }  
                      }
                    break;
                case "show":
                    break;
                case "play":
                    break;             
                default:
                    break;
            }
        }

        public override bool ProcessKeyboard(Keyboard state)
        {
            if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.C))
            {
                DoAction("create");
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.S))
            {
                DoAction("send");
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.B))
            {
                DoAction("bind");
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.P))
            {
                controller.PrintMogwaiKeys();
                LogInConsole("TASK", "prinited mogwaikeys into a file.");
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.T))
            {
                controller.Tag();
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


        public void LogInConsole(string type, string msg)
        {
            string color = "khaki";
            if (type == "DONE")
            {
                color = "limegreen";
            } else if (type == "FAIL")
            {
                color = "red";
            }
            var time = DateTime.Now.ToLocalTime().ToLongTimeString();
            logConsole.Cursor.Print($"[c:r f:{color}]{time}[[c:r f:khaki]{type}[c:r f:{color}]]:[c:r f:gray] {msg}");
            logConsole.Cursor.NewLine();
        }

        public override void Update(TimeSpan delta)
        {
            if (IsReady)
            {

                decimal deposit = controller.GetDepositFunds();
                var depositStr = deposit < 10000 ? deposit.ToString("###0.0000").PadLeft(9) : "TYCOON".PadRight(9);
                var lastBlock = controller.WalletLastBlock;
                if (lastBlock != null)
                {
                    Print(1, 0, controller.WalletLastBlock.Height.ToString("#######0").PadLeft(8) + " Block", Color.Gainsboro);
                    DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    var currentTime = epoch.AddSeconds(controller.WalletLastBlock.Time);
                    var localTime = currentTime.ToLocalTime();
                    TimeSpan t = DateTime.Now.Subtract(localTime);
                    var localtimeStr = localTime.ToString();
                    Print(16, 0, localtimeStr + " [   s]", Color.Gainsboro);
                    Print(18 + localtimeStr.Length, 0, t.TotalSeconds.ToString("##0").PadLeft(3), Color.SpringGreen);
                }
                Print(45, 0, "Funds:", Color.DarkCyan);
                Print(52, 0, depositStr, Color.Orange);

                // only updated if we have keys
                if (controller.HasMogwayKeys)
                {
                    var list = MogwaiKeysList;
                    for (int i = 0; i < list.Count; i++)
                    {
                        var mogwaiKeys = list[i];
                        PrintRow(i + headerPosition + 1, mogwaiKeys, i == pointer, controller.TaggedMogwaiKeys.Contains(mogwaiKeys));
                    }
                    //PrintRow(pointer + headerPosition + 1, list[pointer], true);
                }
            }
            base.Update(delta);
        }

        private void PrintRow(int index, MogwaiKeys mogwaiKeys, bool selected = false, bool tagged = false)
        {
            int aPos = 4;
            int sPos = 41;
            int fPos = 50;
            int nPos = 62;
            int rPos = 76;
            int lPos = 86;
            int gPos = 95;
            var balance = mogwaiKeys.Balance;
            var balanceStr = balance < 1000 ? balance.ToString("##0.0000").PadLeft(8) : "RICH".PadRight(8);
            var mogwai = mogwaiKeys.Mogwai;
            var nameStr = mogwai != null ? mogwai.Name.PadRight(11) : "".PadRight(11, '.');
            var rateStr = mogwai != null ? mogwai.Rating.ToString("#0.00").PadRight(7) : "".PadRight(7, '.');
            var levlStr = mogwai != null ? mogwai.CurrentLevel.ToString("##0").PadRight(5) : "".PadRight(5, '.');
            var goldStr = mogwai != null ? mogwai.Wealth.Gold.ToString("#####0.00").PadRight(10) : "".PadRight(10, '.');

            Print(0, index, !tagged ? " " : "*", !tagged ? Color.Black : Color.SteelBlue);
            Print(1, index, !selected ? "  " : "=>", !selected ? Color.Black : Color.SpringGreen);

            Color standard = GetColorStandard(mogwaiKeys.MogwaiKeysState, selected);
            Color extState = GetMogwaiKeysStateColor(mogwaiKeys.MogwaiKeysState, selected);

            Print(aPos, index, mogwaiKeys.Address.PadRight(36), standard);
            Print(sPos, index, mogwaiKeys.MogwaiKeysState.ToString().PadRight(6), extState);
            Print(fPos, index, balanceStr, extState);
            Print(nPos, index, nameStr, standard);
            Print(rPos, index, rateStr, standard);
            Print(lPos, index, levlStr, standard);
            Print(gPos, index, goldStr, standard);

            //var str = mogwaiKeys.Address.PadRight(36)
            //+ mogwaiKeys.Balance.ToString("####0.00").PadRight(10)
            //+ mogwaiKeys.Shifts?.Count.ToString().PadRight(10)
            //+ mogwaiKeys.Mogwai?.Name.ToString().PadRight(10);
            //Print(1, index, str, selected ? Color.Cyan : Color.DarkCyan);
        }

        private Color GetColorStandard(MogwaiKeysState mogwaiKeysState, bool Selected)
        {
            switch (mogwaiKeysState)
            {
                case MogwaiKeysState.NONE:
                    return Selected ? Color.WhiteSmoke : Color.DarkGray;
                case MogwaiKeysState.WAIT:
                    return Selected ? Color.WhiteSmoke : Color.DarkGray;
                case MogwaiKeysState.READY:
                    return Selected ? Color.Sienna : Color.SaddleBrown;
                case MogwaiKeysState.CREATE:
                    return Selected ? Color.Gold : Color.DarkGoldenrod;
                case MogwaiKeysState.BOUND:
                    return Selected ? Color.Gold : Color.DarkGoldenrod;
                default:
                    return Color.MediumSeaGreen;
            }
        }

        private Color GetMogwaiKeysStateColor(MogwaiKeysState mogwaiKeysState, bool Selected)
        {
            switch (mogwaiKeysState)
            {
                case MogwaiKeysState.NONE:
                    return Selected ? Color.Red : Color.DarkRed;
                case MogwaiKeysState.WAIT:
                    return Selected ? Color.RoyalBlue : Color.SteelBlue;
                case MogwaiKeysState.READY:
                    return Selected ? Color.LimeGreen : Color.DarkGreen;
                case MogwaiKeysState.CREATE:
                    return Selected ? Color.RoyalBlue : Color.SteelBlue;
                case MogwaiKeysState.BOUND:
                    return Selected ? Color.Gold : Color.DarkGoldenrod;
                default:
                    return Color.RoyalBlue;
            }
        }
    }
}
