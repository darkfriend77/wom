using DearMogwai.Application.Engine.UiObjects;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using Veldrid;

namespace DearMogwai.Application.Engine.Subsystems
{
    public class Ui : ISubSystem
    {
        private readonly Engine _runner;
        private Sdl2Renderer _renderer;
        private FrameTimer _ft;
        private ImGuiRenderer _imguiRenderer;

        public List<IUiWindow> Windows { get; } = new List<IUiWindow>();

        public static void RenderMenuEntry(MenuEntry current, Dictionary<MenuEntry, List<MenuEntry>> groupedEntries)
        {
            if (groupedEntries.ContainsKey(current))
            {
                if (ImGui.BeginMenu(current.ComputedLabel, current.IsEnabled?.Invoke() ?? true))
                {
                    foreach (var e in groupedEntries[current])
                    {
                        RenderMenuEntry(e, groupedEntries);
                    }
                    ImGui.EndMenu();
                }
            }
            else
            {
                if (ImGui.MenuItem(current.ComputedLabel, "", current.IsSelected?.Invoke() ?? false, current.IsEnabled?.Invoke() ?? true))
                {
                    current.Action?.Invoke();
                }
            }
        }

        public static void RenderMenu(IEnumerable<IGrouping<MenuEntry, MenuEntry>> groupedEntries)
        {
            var enumerable = groupedEntries as IGrouping<MenuEntry, MenuEntry>[] ?? groupedEntries.ToArray();
            var groups = enumerable.Where(g => g.Key != null).ToDictionary(g => g.Key, g => g.ToList());
            foreach (var e in enumerable.First(g => g.Key == null))
            {
                RenderMenuEntry(e, groups);
            }
        }

        public void RenderCallback(GraphicsDevice gd, CommandList cl)
        {
            _imguiRenderer.Render(gd, cl);
        }

        /// <summary>
        /// Gets or creates a handle for a texture to be drawn with ImGui.
        /// Pass the returned handle to Image() or ImageButton().
        /// </summary>
        public IntPtr GetOrCreateImGuiBinding(ResourceFactory factory, TextureView textureView)
        {
            return _imguiRenderer.GetOrCreateImGuiBinding(factory, textureView);
        }

        public Ui(Engine runner)
        {
            _runner = runner;
            _runner.RegisterSubsystem(this);
        }

        #region ISubSystem members
        public int StartupOrder => 3;
        public int TickOrder => Int32.MaxValue - 1;

        public void Start()
        {
            _renderer = _runner.GetSubSystem<Sdl2Renderer>();
            _ft = _runner.GetSubSystem<FrameTimer>();

            if (_imguiRenderer == null)
            {
                _imguiRenderer = new ImGuiRenderer(_renderer.GraphicsDevice, _renderer.MainSwapchain.Framebuffer.OutputDescription, (int)_renderer.Size.X, (int)_renderer.Size.Y);
            }
            else
            {
                _imguiRenderer.CreateDeviceResources(_renderer.GraphicsDevice, _renderer.MainSwapchain.Framebuffer.OutputDescription);
            }

            _renderer.Render += RenderCallback;
        }

        public void Tick()
        {
            _imguiRenderer.Update(_ft.FrameDelta, _renderer.LastInput);
            var grouped = Windows.SelectMany(w => w.MainMenuEntries).GroupBy(e => e.Parent);
            var groupedEntries = grouped as IGrouping<MenuEntry, MenuEntry>[] ?? grouped.ToArray();
            if (groupedEntries.Any())
            {
                if (ImGui.BeginMainMenuBar())
                {
                    RenderMenu(groupedEntries);
                    ImGui.EndMainMenuBar();
                }
            }
            foreach (var window in Windows)
            {
                window.Render();
            }
        }

        public void Shutdown()
        {
            _renderer.Render -= RenderCallback;
            _imguiRenderer.Dispose();
            _imguiRenderer = null;
        }
        #endregion
    }
}
