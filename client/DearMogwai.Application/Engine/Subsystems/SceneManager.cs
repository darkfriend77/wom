using AssetPrimitives;
using DearMogwai.Application.Engine.SceneObjects;
using DearMogwai.Application.Engine.UiObjects;
using DearMogwai.Application.Game.Scenes;
using System.Threading.Tasks;
using Veldrid;

namespace DearMogwai.Application.Engine.Subsystems
{
    public class SceneManager : ISubSystem
    {
        private readonly Engine _runner;
        private readonly string _splashScreen;
        private Sdl2Renderer _renderer;
        private Ui _ui;
        private IScene _currentScene;
        private ProcessedTexture _loadingScreenTexture;
        private volatile bool _isLoading = false;

        private LoadingScreen _loadingScreen;

        private Texture _loadingTexture;
        private TextureView _loadingTextureView;
        public SceneManager(Engine runner, string splashScreen)
        {
            _runner = runner;
            _runner.RegisterSubsystem(this);
            _splashScreen = splashScreen;
        }

        public void LoadScene(IScene scene)
        {
            _isLoading = true;
            Task.Run(() =>
            {
                if (_currentScene != null)
                {
                    _renderer.Render -= _currentScene.Render;
                    _currentScene.Shutdown();
                }

                _currentScene = scene;
                _currentScene.Start(_runner);
                _renderer.Render += _currentScene.Render;
                _isLoading = false;
            });
        }

        #region ISubSystem members
        public int StartupOrder => 4;
        public int TickOrder => 4;
        public void Start()
        {
            _renderer = _runner.GetSubSystem<Sdl2Renderer>();
            _ui = _runner.GetSubSystem<Ui>();
            _loadingScreenTexture = _runner.GetSubSystem<ResourceLoader>().LoadEmbeddedAsset<ProcessedTexture>(_splashScreen);

            _loadingTexture = _loadingScreenTexture.CreateDeviceTexture(_renderer.GraphicsDevice, _renderer.ResourceFactory, TextureUsage.Sampled);
            _loadingTextureView = _renderer.ResourceFactory.CreateTextureView(_loadingTexture);

            _loadingScreen = new LoadingScreen(_ui.GetOrCreateImGuiBinding(_renderer.ResourceFactory, _loadingTextureView));

            _ui.Windows.Add(_loadingScreen);

            LoadScene(new MenuScene());
        }

        public void Tick()
        {
            _loadingScreen.IsVisible = _isLoading;
            _currentScene?.Update();
        }

        public void Shutdown()
        {
            _currentScene?.Shutdown();
            _ui.Windows.Remove(_loadingScreen);
            _loadingScreen = null;
            _loadingTextureView.Dispose();
            _loadingTextureView = null;
            _loadingTexture.Dispose();
            _loadingTexture = null;
        }
        #endregion
    }
}
