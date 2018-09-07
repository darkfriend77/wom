using System.Collections.Generic;
using System.Numerics;
using Veldrid;

namespace DearMogwai.Application.Engine.Subsystems
{
    public class Input : ISubSystem
    {
        private readonly Engine _runner;
        private readonly HashSet<Key> _currentlyPressedKeys = new HashSet<Key>();
        private readonly HashSet<Key> _newKeysThisFrame = new HashSet<Key>();
        private readonly HashSet<MouseButton> _currentlyPressedMouseButtons = new HashSet<MouseButton>();
        private readonly HashSet<MouseButton> _newMouseButtonsThisFrame = new HashSet<MouseButton>();
        private Sdl2Renderer _renderer;

        public Vector2 MousePosition { get; private set; }

        public bool IsKeyPressed(Key key)
        {
            return _currentlyPressedKeys.Contains(key);
        }

        public bool IsKeyDown(Key key)
        {
            return _newKeysThisFrame.Contains(key);
        }

        public bool IsMousePressed(MouseButton button)
        {
            return _currentlyPressedMouseButtons.Contains(button);
        }

        public bool IsMouseDown(MouseButton button)
        {
            return _newMouseButtonsThisFrame.Contains(button);
        }

        public Input(Engine runner)
        {
            _runner = runner;
            _runner.RegisterSubsystem(this);
        }

        private void MouseUp(MouseButton mouseButton)
        {
            _currentlyPressedMouseButtons.Remove(mouseButton);
            _newMouseButtonsThisFrame.Remove(mouseButton);
        }

        private void MouseDown(MouseButton mouseButton)
        {
            if (_currentlyPressedMouseButtons.Add(mouseButton))
            {
                _newMouseButtonsThisFrame.Add(mouseButton);
            }
        }

        private void KeyUp(Key key)
        {
            _currentlyPressedKeys.Remove(key);
            _newKeysThisFrame.Remove(key);
        }

        private void KeyDown(Key key)
        {
            if (_currentlyPressedKeys.Add(key))
            {
                _newKeysThisFrame.Add(key);
            }
        }

        #region ISubSystem members
        public int StartupOrder => 2;
        public int TickOrder => 1;
        public void Start()
        {
            _renderer = _runner.GetSubSystem<Sdl2Renderer>();
        }

        public void Tick()
        {
            _newKeysThisFrame.Clear();
            _newMouseButtonsThisFrame.Clear();

            MousePosition = _renderer.LastInput.MousePosition;
            foreach (var ke in _renderer.LastInput.KeyEvents)
            {
                if (ke.Down)
                {
                    KeyDown(ke.Key);
                }
                else
                {
                    KeyUp(ke.Key);
                }
            }
            foreach (var me in _renderer.LastInput.MouseEvents)
            {
                if (me.Down)
                {
                    MouseDown(me.MouseButton);
                }
                else
                {
                    MouseUp(me.MouseButton);
                }
            }
        }

        public void Shutdown()
        {
            _renderer = null;
        }
        #endregion
    }
}
