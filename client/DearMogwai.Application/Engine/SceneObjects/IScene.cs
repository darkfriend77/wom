using Veldrid;

namespace DearMogwai.Application.Engine.SceneObjects
{
    public interface IScene
    {
        void Start(Engine runner);
        void Update();
        void Render(GraphicsDevice gd, CommandList cl);
        void Shutdown();
    }
}
