using DearMogwai.Application.Engine.SceneObjects;
using DearMogwai.Application.Game.Ui;
using System.Threading;
using Veldrid;

namespace DearMogwai.Application.Game.Scenes
{
    public class MenuScene : IScene
    {
        private Engine.Engine _runner;
        private CharacterSelection _characterSelection;

        public void Start(Engine.Engine runner)
        {
            _runner = runner;

            // Just to keep you waiting and show the loadscreen... :p
            Thread.Sleep(1000);

            _characterSelection = new CharacterSelection(_runner);
            _runner.GetSubSystem<Engine.Subsystems.Ui>().Windows.Add(_characterSelection);
        }

        public void Update()
        {
        }

        public void Render(GraphicsDevice gd, CommandList cl)
        {
        }

        public void Shutdown()
        {
            _runner.GetSubSystem<Engine.Subsystems.Ui>().Windows.Remove(_characterSelection);
            _characterSelection = null;
            _runner = null;
        }
    }
}
