namespace DearMogwai.Application.Engine
{
    public interface ISubSystem
    {
        int StartupOrder { get; }
        int TickOrder { get; }
        void Start();
        void Tick();
        void Shutdown();
    }
}
