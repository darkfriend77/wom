using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using ImGuiNET;

namespace DearMogwai.Application.Engine.UiObjects
{
    class LoadingScreen : IUiWindow
    {
        private readonly IntPtr _splashScreenId;
        public LoadingScreen(IntPtr splashScreenId)
        {
            _splashScreenId = splashScreenId;
        }

        public IEnumerable<MenuEntry> MainMenuEntries { get; } = new List<MenuEntry>();
        public bool IsVisible { get; set; } = true;
        public void Update(float deltaSeconds)
        {
        }

        public void Render()
        {
            bool iv = IsVisible;

            if(iv) ImGui.OpenPopup("Loading");

            if (ImGui.BeginPopupModal("Loading", ref iv,
                WindowFlags.NoMove | WindowFlags.NoResize | WindowFlags.AlwaysAutoResize | WindowFlags.NoCollapse |
                WindowFlags.NoInputs | WindowFlags.NoTitleBar | WindowFlags.NoSavedSettings))
            {
                ImGui.Image(_splashScreenId, new Vector2(1000, 638), Vector2.Zero, Vector2.One, Vector4.One, Vector4.Zero);
                ImGui.EndPopup();
            }
        }
    }
}
