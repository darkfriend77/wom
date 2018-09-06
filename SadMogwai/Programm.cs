using System;
using SadConsole;
using Console = SadConsole.Console;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SadMogwai.Consoles;
using SadMogwai.Art;
using NAudio.Wave;
using System.Threading;

namespace SadMogwai
{
    class Program
    {

        public const int Width = 140;
        public const int Height = 40;

        private static WaveOutEvent _outputDevice;

        private static SplashScreen _splashScreen;
        private static SelectionConsole _selectionConsole;
        private static MogwaiConsole _welcome;
        private static MogwaiConsole _info;
        private static MogwaiConsole _command;

        static void Main(string[] args)
        {
            // Setup the engine and creat the main window.
            SadConsole.Game.Create("IBM.font", Width, Height);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Hook the update event that happens each frame so we can trap keys and respond.
            SadConsole.Game.OnUpdate = Update;

            // Start the game.
            SadConsole.Game.Instance.Run();

            //
            // Code here will not run until the game window closes.
            //

            SadConsole.Game.Instance.Dispose();
        }

        private static void Update(GameTime time)
        {
            // Called each logic update.

            // As an example, we'll use the F5 key to make the game full screen
            if (Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F5))
            {
                Settings.ToggleFullScreen();
            }
        }

        private static void Init()
        {
            // Any custom loading and prep. We will use a sample console for now

            //var audioFile = new AudioFileReader("mogwaimusic.mp3");
            //_outputDevice = new WaveOutEvent();
            //_outputDevice.Init(audioFile);
            //_outputDevice.Play();

            _welcome = new MogwaiConsole("Welcome", "Mogwaicoin Team 2018", 110, 6)
            {
                Position = new Point(1, 1)
            };
            for (int i = 0; i < Ascii.Logo.Length; i++)
            {
                string str = Ascii.Logo[i];
                _welcome.Print(4, i, str, Color.Cyan, Color.Black);
            }

            _selectionConsole = new SelectionConsole("Selection", "", 110, 25)
            {
                Position = new Point(1, 9)
            };
            _info = new MogwaiConsole("Info", "", 24, 38)
            {
                Position = new Point(114, 1)
            };

            _command = new MogwaiConsole("Command", "", 110, 3)
            {
                Position = new Point(1, 36)
            };


            //_splashScreen = new SplashScreen(140, 30);
            //_splashScreen.IsVisible = true;
            //_splashScreen.SplashCompleted += SplashScreenCompleted;
            //Global.CurrentScreen.Children.Add(_splashScreen);

            // Set our new console as the thing to render and process
            Global.CurrentScreen.Children.Add(_welcome);
            Global.CurrentScreen.Children.Add(_selectionConsole);
            Global.CurrentScreen.Children.Add(_info);
            Global.CurrentScreen.Children.Add(_command);
        }

        private static void SplashScreenCompleted()
        {
            Global.CurrentScreen.Children.Clear();
            Global.CurrentScreen.Children.Add(_welcome);
            Global.CurrentScreen.Children.Add(_selectionConsole);
            Global.CurrentScreen.Children.Add(_info);
            Global.CurrentScreen.Children.Add(_command);
        }
    }
}
