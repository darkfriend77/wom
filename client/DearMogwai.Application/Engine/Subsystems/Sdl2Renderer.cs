using System;
using System.Numerics;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using Veldrid.Utilities;

namespace DearMogwai.Application.Engine.Subsystems
{
    public class Sdl2Renderer : ISubSystem
    {
        private readonly Engine _runner;
        private FrameTimer _ft;
        private ResourceLoader _loader;
        private Sdl2Window _window;
        private GraphicsDevice _gd;
        private Pipeline _pipeline;
        private DisposeCollectorResourceFactory _factory;
        private bool _windowResized;
        private CommandList _cl;

        public InputSnapshot LastInput { get; private set; }
        public GraphicsDevice GraphicsDevice => _gd;
        public ResourceFactory ResourceFactory => _factory;
        public Swapchain MainSwapchain => _gd.MainSwapchain;
        public Framebuffer MainFramebuffer => MainSwapchain.Framebuffer;
        public Vector2 Size => new Vector2(_window.Width, _window.Height);
        public event Action<GraphicsDevice, CommandList> Render;

        public Sdl2Renderer(Engine runner)
        {
            _runner = runner;
            _runner.RegisterSubsystem(this);
        }

        #region ISubSystem members
        public int StartupOrder => 1;
        public int TickOrder => Int32.MaxValue;

        public void Start()
        {
            WindowCreateInfo wci = new WindowCreateInfo
            {
                X = 100,
                Y = 100,
                WindowWidth = 1280,
                WindowHeight = 720,
                WindowTitle = _runner.Name,
            };
            
            _window = VeldridStartup.CreateWindow(ref wci);
            _window.Resized += () =>
            {
                _windowResized = true;
            };

            _ft = _runner.GetSubSystem<FrameTimer>();
            _loader = _runner.GetSubSystem<ResourceLoader>();

            GraphicsDeviceOptions options = new GraphicsDeviceOptions(
                debug: false,
                swapchainDepthFormat: PixelFormat.R16_UNorm,
                syncToVerticalBlank: true,
                resourceBindingModel: ResourceBindingModel.Improved);
#if DEBUG
            options.Debug = true;
#endif
            _gd = VeldridStartup.CreateGraphicsDevice(_window, options);
            _factory = new DisposeCollectorResourceFactory(_gd.ResourceFactory);

            ResourceLayout projViewLayout = _factory.CreateResourceLayout(
                new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription("Projection", ResourceKind.UniformBuffer, ShaderStages.Vertex),
                    new ResourceLayoutElementDescription("View", ResourceKind.UniformBuffer, ShaderStages.Vertex)));

            ResourceLayout worldTextureLayout = _factory.CreateResourceLayout(
                new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription("World", ResourceKind.UniformBuffer, ShaderStages.Vertex),
                    new ResourceLayoutElementDescription("SurfaceTexture", ResourceKind.TextureReadOnly,
                        ShaderStages.Fragment),
                    new ResourceLayoutElementDescription("SurfaceSampler", ResourceKind.Sampler,
                        ShaderStages.Fragment)));

            ShaderSetDescription shaderSet = new ShaderSetDescription(
                new[]
                {
                    new VertexLayoutDescription(
                        new VertexElementDescription("Position", VertexElementSemantic.Position,
                            VertexElementFormat.Float3),
                        new VertexElementDescription("TexCoords", VertexElementSemantic.TextureCoordinate,
                            VertexElementFormat.Float2))
                },
                new[]
                {
                    _loader.LoadShader(_factory, "Cube", ShaderStages.Vertex, "VS"),
                    _loader.LoadShader(_factory, "Cube", ShaderStages.Fragment, "FS")
                });

            _pipeline = _factory.CreateGraphicsPipeline(new GraphicsPipelineDescription(
                BlendStateDescription.SingleOverrideBlend,
                DepthStencilStateDescription.DepthOnlyLessEqual,
                RasterizerStateDescription.Default,
                PrimitiveTopology.TriangleList,
                shaderSet,
                new[] { projViewLayout, worldTextureLayout },
                MainSwapchain.Framebuffer.OutputDescription));

            _cl = _factory.CreateCommandList();
            LastInput = _window.PumpEvents();
        }

        public void Tick()
        {
            if (!_window.Exists)
            {
                _runner.Shutdown();
                return;
            }

            _cl.Begin();

            _cl.SetFramebuffer(MainSwapchain.Framebuffer);
            _cl.ClearColorTarget(0, RgbaFloat.Black);
            _cl.ClearDepthStencil(1f);
            _cl.SetPipeline(_pipeline);

            LastInput = _window.PumpEvents();
            if (_windowResized)
            {
                _windowResized = false;
                _gd.ResizeMainWindow((uint)_window.Width, (uint)_window.Height);
            }

            Render?.Invoke(_gd, _cl);

            _cl.End();
            GraphicsDevice.SubmitCommands(_cl);
            GraphicsDevice.SwapBuffers(MainSwapchain);
            GraphicsDevice.WaitForIdle();
        }

        public void Shutdown()
        {
            _gd.WaitForIdle();
            _factory.DisposeCollector.DisposeAll();
            _factory = null;
            _gd.Dispose();
            _gd = null;
            _window.Close();
            _window = null;
        }
        #endregion
    }
}
