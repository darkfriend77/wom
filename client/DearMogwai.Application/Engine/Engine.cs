using System.Collections.Generic;
using System.Linq;

namespace DearMogwai.Application.Engine
{
    public class Engine
    {
        private List<ISubSystem> SubSystems { get; } = new List<ISubSystem>();

        public string Name { get; protected set; }
        public bool IsRunning { get; private set; }
        public PlatformType Platform { get; }

        public Engine()
        {
            Platform = PlatformType.Desktop;
        }

        public void RegisterSubsystem(ISubSystem subSystem)
        {
            SubSystems.Add(subSystem);
        }

        public T GetSubSystem<T>() where T : ISubSystem
        {
            return SubSystems.OfType<T>().FirstOrDefault();
        }

        public void Run()
        {
            IsRunning = true;

            var startupSystems = SubSystems.OrderBy(s => s.StartupOrder);
            Queue<ISubSystem> shutdownQueue = new Queue<ISubSystem>();
            foreach (var startupSystem in startupSystems)
            {
                startupSystem.Start();
                shutdownQueue.Enqueue(startupSystem);
            }

            var tickSystems = SubSystems.OrderBy(s => s.TickOrder).ToList();
            while (IsRunning)
            {
                foreach (var tickSystem in tickSystems)
                {
                    tickSystem.Tick();
                }
            }

            while (shutdownQueue.TryDequeue(out ISubSystem shutdownSystem))
            {
                shutdownSystem.Shutdown();
            }
        }

        public void Shutdown()
        {
            IsRunning = false;
        }
    }
}
