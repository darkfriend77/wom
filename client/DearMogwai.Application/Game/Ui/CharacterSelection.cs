using DearMogwai.API.GameObjects;
using DearMogwai.Application.Engine;
using DearMogwai.Application.Engine.UiObjects;
using ImGuiNET;
using System.Collections.Generic;
using System.Numerics;

namespace DearMogwai.Application.Game.Ui
{
    public class CharacterSelection : IUiWindow
    {
        private readonly Engine.Engine _runner;
        private readonly List<CharacterOverview> _availableCharacters;
        private CharacterOverview _selectedCharacter;
        public IEnumerable<MenuEntry> MainMenuEntries { get; }
        public bool IsVisible { get; set; } = true;

        public CharacterSelection(Engine.Engine runner)
        {
            _runner = runner;
            _availableCharacters = new List<CharacterOverview>(new[]{
                new CharacterOverview
                {
                    Address = "Test1",
                    Funds = (decimal)3.5,
                    Gold = (decimal)0.1,
                    IsBound = true,
                    Level = 1,
                    Name = "Test Name",
                    Rating = (decimal)3.5
                },
                new CharacterOverview
                {
                    Address = "Test2",
                    Funds = (decimal)3.4515,
                    Gold = (decimal)0.0,
                    IsBound = true,
                    Level = 2,
                    Name = "Test2 Name",
                    Rating = (decimal)15.6
                }
            });
            _selectedCharacter = null;

            var fileMenu = new MenuEntry("File", "m_File", null);

            MainMenuEntries = new List<MenuEntry>(new[]
            {
                fileMenu,
                new MenuEntry("Character Selection", "m_CharSel", fileMenu, () => { IsVisible = !IsVisible; }, () => IsVisible , () => true),
            });
        }

        public void Update(float deltaSeconds)
        {

        }

        public void Render()
        {
            if (IsVisible)
            {
                ImGui.SetNextWindowPos(
                    new Vector2(ImGui.GetIO().DisplaySize.X * 0.5f, ImGui.GetIO().DisplaySize.Y * 0.5f),
                    Condition.Appearing,
                    new Vector2(0.5f, 0.5f));

                bool iv = IsVisible;
                ImGui.BeginWindow("Character Selection", ref iv, new Vector2(600, 400), 0.8f, WindowFlags.NoCollapse);
                IsVisible = iv;
                
                _availableCharacters.Table("Address", ref _selectedCharacter);

                ImGui.EndWindow();
            }
        }
    }
}
