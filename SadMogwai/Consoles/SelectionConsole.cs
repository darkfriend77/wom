using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using SadConsole.Surfaces;
using System;
using System.Collections.Generic;

namespace SadMogwai.Consoles
{
    public class SelectionConsole : SadConsole.Console
    {

        private int oldPointer, pointer;

        private Dictionary<int, string> itemDic;

        private Color rowNormal = Color.DarkCyan;

        private Color highNormal = Color.Cyan;

        private Basic borderSurface;

        private int glyphIndex = 185;

        public SelectionConsole(int width, int height) : base(width, height)
        {

            borderSurface = new Basic(width + 2, height + 2, base.Font);
            borderSurface.DrawBox(new Rectangle(0, 0, borderSurface.Width, borderSurface.Height),
                                  new Cell(Color.DarkCyan, Color.Black), null, SurfaceBase.ConnectedLineThick);
            borderSurface.Position = new Point(-1, -1);
            CreateHeader();
            Children.Add(borderSurface);

            pointer = 0;
            oldPointer = 0;


            itemDic = new Dictionary<int, string>
            {
                {  0, "    M8NEb9uFUhGp4obMdouNLncET4LCfAroP8  Bound      3.9894   Cabufudae       11.71        1         0" },
                {  1, "    MAXcZCjBXYaJpXEpgz7P1aYSv4eHqAs1ig  Bound      3.9999   Wygukuqu        12.86        1         0" },
                {  2, "    MCmpMFvQXeGQxJSSdCuPEf58v5iePJesN5  Bound      3.9999   Negalake        12.93        1         0" },
                {  3, "    MDpWXpuXP8RE6xx6Et4XR7nGB8pTZo271c  Bound      3.9893   Jalyquno        14.93        1         0" },
                {  4, "    MFTHxujEGC7AHNBMCWQCuXZgVurWjLKc5e  Bound      8.9894   Ckuckari        11.93        1         0" },
                {  5, "    MFZqCvi1A95pR83K4itkaVFAnvwYfY8CeU  Bound      3.9999   Cusejaxy        12.36        1         0" },
                {  6, "    MG2nFaQ15YH3ExXGVBkHgoVWabPpgu8Lyy  Bound      3.9999   Tapomili        11.93        1         0" },
                {  7, "    MJHYMxu2kyR1Bi4pYwktbeCM7yjZyVxt2i  Bound      3.9894   Doguwutae       14.07        1         0" },
                {  8, "    MJmeexMYULCnqMkBCBs84cPmaCtcg3WBf4  Bound      3.9895   Dushuluzh       11.79        1         0" },
                {  9, "    MK1zwdxSzJ978XH9a2Xojjhbn5tNvnjiUp  None       0.0000   ...               ...      ...       ..." },
                { 10, "    MMtehEJH9xYdmie5zzQE59HDGghZG49RYd  Bound      3.9999   Sheshewo        12.57        1         0" },
                { 11, "    MNt4c32XybpMJYX35kTyK6Hq6pzDW9znoB  None       0.0000   ...               ...      ...       ..." },
                { 12, "    MP472HbD9dmrddTW1V6gh8bzfFijdFdqdB  Bound      3.9999   Rorepiba        12.86        1         0" },
                { 13, "    MRThQvrcykzGRfpgM73mmjiiHiEjRXb4wQ  Bound      3.9999   Xoteckev        10.71        1         0" }
            };

            Init();
        }

        private void CreateHeader()
        {
            //borderSurface.Print(1, 0, "[c:sg 205:108]".PadRight(122), Color.DarkCyan);

            borderSurface.SetGlyph(0, 0, 204, Color.DarkCyan);
            borderSurface.SetGlyph(111, 0, 185, Color.DarkCyan);
            borderSurface.SetGlyph(03, 0, 185, Color.DarkCyan);
            borderSurface.Print(04, 0, " Address ");
            borderSurface.SetGlyph(39, 0, 185, Color.DarkCyan);
            borderSurface.Print(40, 0, " State ");
            borderSurface.SetGlyph(48, 0, 185, Color.DarkCyan);
            borderSurface.Print(49, 0, " Funds ");
            borderSurface.SetGlyph(60, 0, 185, Color.DarkCyan);
            borderSurface.Print(61, 0, " Name ");
            borderSurface.SetGlyph(74, 0, 185, Color.DarkCyan);
            borderSurface.Print(75, 0, " Rating ");
            borderSurface.SetGlyph(84, 0, 185, Color.DarkCyan);
            borderSurface.Print(85, 0, " Level ");
            borderSurface.SetGlyph(93, 0, 185, Color.DarkCyan);
            borderSurface.Print(94, 0, " Gold ");
        }

        private void Init()
        {
            for (int i = 0; i < itemDic.Count; i++)
            {
                Print(1, i + 1, itemDic[i], i == pointer ? highNormal : rowNormal, Color.Black);
            }
        }

        private void UpdateRow()
        {
            if (oldPointer != pointer)
            {
                Print(1, oldPointer + 1, itemDic[oldPointer], rowNormal, Color.Black);
                Print(1, pointer + 1, itemDic[pointer], highNormal, Color.Black);
            }
        }

        public override bool ProcessKeyboard(Keyboard state)
        {
            if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                return true;
            }
            else if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                if (pointer < 10)
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
            base.Update(delta);

            if (ProcessKeyboard(Global.KeyboardState))
            {
                UpdateRow();
            };
        }
    }
}
