using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using System;
using System.Collections.Generic;

namespace SadMogwai.Consoles
{
    public class SelectionConsole : MogwaiConsole
    {

        private int pointer;

        private Dictionary<int, string> itemDic;

        public SelectionConsole(string title, string footer, int width, int height) : base(title, footer, width, height)
        {
            pointer = 0;
            itemDic = new Dictionary<int, string>();
            itemDic.Add(0, "And thats it. The console now has the ability to scroll the designated height buffer.");
            itemDic.Add(1, "And thats it. The console now has the ability to scroll the designated height buffer.");
            itemDic.Add(2, "And thats it. The console now has the ability to scroll the designated height buffer.");
            itemDic.Add(3, "And thats it. The console now has the ability to scroll the designated height buffer.");
            itemDic.Add(4, "And thats it. The console now has the ability to scroll the designated height buffer.");
            itemDic.Add(5, "And thats it. The console now has the ability to scroll the designated height buffer.");
            itemDic.Add(6, "And thats it. The console now has the ability to scroll the designated height buffer.");
            itemDic.Add(7, "And thats it. The console now has the ability to scroll the designated height buffer.");
            itemDic.Add(8, "And thats it. The console now has the ability to scroll the designated height buffer.");
            itemDic.Add(9, "And thats it. The console now has the ability to scroll the designated height buffer.");
            itemDic.Add(10, "And thats it. The console now has the ability to scroll the designated height buffer.");
            itemDic.Add(11, "And thats it. The console now has the ability to scroll the designated height buffer.");
            itemDic.Add(12, "And thats it. The console now has the ability to scroll the designated height buffer.");
            itemDic.Add(13, "And thats it. The console now has the ability to scroll the designated height buffer.");
            itemDic.Add(14, "And thats it. The console now has the ability to scroll the designated height buffer.");
            itemDic.Add(15, "And thats it. The console now has the ability to scroll the designated height buffer.");
            itemDic.Add(16, "And thats it. The console now has the ability to scroll the designated height buffer.");
            itemDic.Add(17, "And thats it. The console now has the ability to scroll the designated height buffer.");
            Update();
        }

        private void Update()
        {
            for (int i = 0; i < itemDic.Count; i++)
            {
                Print(1, i, itemDic[i]);
            }

        }

        public override bool ProcessKeyboard(Keyboard state)
        {
            if (state.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                return true;
            }

            return false;
        }

        public override void Update(TimeSpan delta)
        {
            base.Update(delta);

            ProcessKeyboard(Global.KeyboardState);
        }

    }
}
