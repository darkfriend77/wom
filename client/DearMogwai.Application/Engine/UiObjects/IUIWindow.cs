using System.Collections.Generic;

namespace DearMogwai.Application.Engine.UiObjects
{
    public interface IUiWindow
    {
        IEnumerable<MenuEntry> MainMenuEntries { get; }
        bool IsVisible { get; set; }
        void Update(float deltaSeconds);
        void Render();
    }
}
