using System;
using System.Diagnostics;

namespace DearMogwai.Application.Engine.Subsystems
{
    public class FrameTimer : ISubSystem
    {
        private Stopwatch _watch;
        private double _previousElapsed;
        public FrameTimer(Engine runner)
        {
            runner.RegisterSubsystem(this);
        }

        public int CurrentFrame { get; private set; }
        public float FrameDelta { get; private set; }
        public TimeSpan Runtime => _watch.Elapsed;

        #region ISubSystem members
        public int StartupOrder { get; } = 0;
        public int TickOrder { get; } = 0;

        public void Start()
        {
            _watch = Stopwatch.StartNew();
            _previousElapsed = _watch.Elapsed.TotalSeconds;
        }

        public void Tick()
        {
            double newElapsed = _watch.Elapsed.TotalSeconds;
            CurrentFrame++;
            FrameDelta = (float)(newElapsed - _previousElapsed);
            _previousElapsed = newElapsed;
        }

        public void Shutdown()
        {
        }
        #endregion
    }
}
