using SampleBase;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace DearMogwai
{
    class Program
    {
        static void Main(string[] args)
        {
            VeldridStartupWindow window = new VeldridStartupWindow("DearMogwai");
            Application.DearMogwai app = new Application.DearMogwai(window);
            window.Run();
        }
    }
}
