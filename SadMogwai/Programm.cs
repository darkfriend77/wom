using System;
using SadConsole;
using Console = SadConsole.Console;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SadMogwai.Consoles;
using SadMogwai.Art;
using NAudio.Wave;
using System.Threading;
using SadMogwai.Dialogs;
using WoMApi.Node;
using System.Reflection;
using log4net;
using log4net.Config;

namespace SadMogwai
{
            
    class Program
    {
         private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public const int Width = 140;
        public const int Height = 40;

        private static WaveOutEvent _outputDevice;

        private static SplashScreen _splashScreen;
        private static SelectionScreen _selectionConsole;
        private static MogwaiConsole _welcome;
        private static MogwaiConsole _info;
        private static MogwaiConsole _command;

        private static SadGuiState _state;

        private static MogwaiController _controller;

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

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
            // As an example, we'll use the F5 key to make the game full screen
            if (Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F5))
            {
                Settings.ToggleFullScreen();
            }
            else if (Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                SadConsole.Game.Instance.Exit();
            }

            //return;

            // Called each logic update.
            switch (_state)
            {
                case SadGuiState.START:
                    _state = LoadBlocksAsync();
                    return;
                case SadGuiState.ACTION:
                    return;
                case SadGuiState.LOGIN:
                    if (!_controller.IsWalletCreated)
                    {
                        _state = CreateWallet();
                    }
                    else
                    {
                        _state = Unlock();
                    }
                    return;
                case SadGuiState.MNEMOIC:
                    _state = ShowMnemoic();
                    break;
                case SadGuiState.SELECTION:
                    if (!_selectionConsole.IsReady)
                    {
                        _selectionConsole.Init();
                        break;
                    }
                    _selectionConsole.ProcessKeyboard(Global.KeyboardState);
                    break;
                case SadGuiState.FATALERROR:
                    _state = Warning("A fatal error happend!", true);
                    break;
                case SadGuiState.QUIT:
                    SadConsole.Game.Instance.Exit();
                    break;
            }

        }

        private static SadGuiState LoadBlocksAsync()
        {
            var dialog = new MogwaiProgressDialog("Loading", "caching all mogwai blocks.",_controller, 40, 8);
            dialog.AddButon("ok");
            dialog.StartAsync();
            dialog.button.Click += (btn, args) =>
            {
                if (dialog.IsComplete)
                {
                    dialog.Hide();
                    _state = SadGuiState.LOGIN;
                }
            };
            dialog.Show(true);

            return SadGuiState.ACTION;
        }

        private static SadGuiState Warning(string warning, bool terminate)
        {
            var dialog = new MogwaiDialog("Warning", warning, 40, 8);
            dialog.AddButon("ok");
            dialog.button.Click += (btn, args) =>
            {
                if (terminate)
                {
                    _state = SadGuiState.QUIT;
                }
                else
                {
                    _state = SadGuiState.SELECTION;
                }
                dialog.Hide();
            };
            dialog.Show(true);

            return SadGuiState.ACTION;
        }

        private static SadGuiState ShowMnemoic()
        {
            var mnemoic = _controller.WalletMnemonicWords;
            var size = mnemoic.Length.ToString();
            var dialog = new MogwaiDialog("Show Mnemoic", "[c:g f:LimeGreen:Orange:" + size + "]" + mnemoic.ToUpper(), 40, 8);
            dialog.AddButon("memorized");
            dialog.button.Click += (btn, args) =>
            {
                _state = SadGuiState.SELECTION;
                dialog.Hide();
            };
            dialog.Show(true);

            return SadGuiState.ACTION;
        }

        private static SadGuiState CreateWallet()
        {
            var inputDialog = new MogwaiInputDialog("WalletCreation", "new wallet password?", 40, 8);
            inputDialog.AddButon("ok");
            inputDialog.button.Click += (btn, args) =>
            {
                string password = inputDialog.input?.Text;
                _controller.CreateWallet(password);
                if (_controller.IsWalletCreated)
                {
                    _state = SadGuiState.MNEMOIC;
                }
                else
                {
                    _state = SadGuiState.FATALERROR;
                }
                inputDialog.Hide();
            };
            inputDialog.Show(true);

            return SadGuiState.ACTION;
        }

        private static SadGuiState Unlock()
        {
            var dialog = new MogwaiInputDialog("UnlockWallet", "wallet password?", 40, 8);
            dialog.AddButon("ok");
            dialog.button.Click += (btn, args) =>
            {
                string password = dialog.input.Text;
                _controller.UnlockWallet(password);
                if (_controller.IsWalletUnlocked)
                {
                    _state = SadGuiState.SELECTION;
                }
                else
                {
                    _state = SadGuiState.FATALERROR;
                }
                dialog.Hide();
            };
            dialog.Show(true);

            return SadGuiState.ACTION;
        }

        private static void Init()
        {
            _controller = new MogwaiController();


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
                _welcome.Print(4, i, $"[c:g b:0,0,0:Black:DarkCyan:DarkGoldenRod:DarkRed:Black:0,0,0:{str.Length}][c:g f:LimeGreen:Orange:{str.Length}]" + str, Color.Cyan, Color.Black);
            }

            _selectionConsole = new SelectionScreen(_controller, 110, 25);
            _selectionConsole.Position = new Point(1, 9);
            _info = new MogwaiConsole("Info", "", 24, 38);
            _info.Position = new Point(114, 1);
            _command = new MogwaiConsole("Command", "", 110, 3);
            _command.Position = new Point(1, 36);


            //_splashScreen = new SplashScreen(140, 30);
            //_splashScreen.IsVisible = true;
            //_splashScreen.SplashCompleted += SplashScreenCompleted;
            //Global.CurrentScreen.Children.Add(_splashScreen);

            // Set our new console as the thing to render and process
            Global.CurrentScreen.Children.Add(_welcome);
            Global.CurrentScreen.Children.Add(_selectionConsole);
            Global.CurrentScreen.Children.Add(_info);
            Global.CurrentScreen.Children.Add(_command);
            _state = SadGuiState.START;
        }

        private static void SplashScreenCompleted()
        {
            Global.CurrentScreen.Children.Clear();
            Global.CurrentScreen.Children.Add(_welcome);
            Global.CurrentScreen.Children.Add(_selectionConsole);
            Global.CurrentScreen.Children.Add(_info);
            Global.CurrentScreen.Children.Add(_command);
            _state = SadGuiState.START;
        }
    }

    enum SadGuiState
    {
        START, LOGIN, ACTION, SELECTION,
        MNEMOIC,
        FATALERROR,
        QUIT
    }
}
