using Caliburn.Micro;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Safir.ViewModels
{
    using Core;
    using Core.Application;
    using Core.Settings;
    using Commands;

    public class MainViewModel : Conductor<object>
    {
        //private readonly ILog _log;

        private readonly ISettingStore _settings;

        private ICommand _play = new PlayCommand();
        private ICommand _pause = new PauseCommand();
        private ICommand _rewind = new RewindCommand();
        private ICommand _fastForward = new FastForwardCommand();
        private ICommand _favorite = new FavoriteCommand();

        public MainViewModel(
            //ILog logger,
            IAppMeta appMeta,
            ISettingStore settings,
            PlaybackViewModel playback,
            MainMenuViewModel mainMenu,
            LibraryMenuViewModel libraryMenu,
            PlaylistMenuViewModel playlistMenu) {
            //_log = logger;
            AppName = appMeta.AppName;
            Version = appMeta.AppName + " v" + appMeta.AppVersion;
            _settings = settings;
            Playback = playback;
            MainMenu = mainMenu;
            LibraryMenu = libraryMenu;
            PlaylistMenu = playlistMenu;
        }

        #region Window Settings

        protected override void OnInitialize() {
            var view = GetView() as Window;
            view.Height = _settings.Get<double>("WindowHeight");
            view.Width = _settings.Get<double>("WindowWidth");
            view.Left = _settings.Get<double>("WindowLeft");
            view.Top = _settings.Get<double>("WindowTop");
            view.WindowState = _settings.Get<WindowState>("WindowState");
        }

        protected override void OnDeactivate(bool close) {
            var view = GetView() as Window;
            _settings.Set("WindowHeight", view.Height);
            _settings.Set("WindowWidth", view.Width);
            _settings.Set("WindowLeft", view.Left);
            _settings.Set("WindowTop", view.Top);
            _settings.Set("WindowState", view.WindowState);
        }

        #endregion

        public string AppName { get; private set; }
        public string Version { get; private set; }

        public PlaybackViewModel Playback { get; set; }

        public MainMenuViewModel MainMenu { get; set; }
        public LibraryMenuViewModel LibraryMenu { get; set; }
        public PlaylistMenuViewModel PlaylistMenu { get; set; }

        public IContentDisplay Content { get; private set; }
        
        public ICommand Play { get { return _play; } }
        public ICommand Pause { get { return _pause; } }
        public ICommand Rewind { get { return _rewind; } }
        public ICommand FastForward { get { return _fastForward; } }
        public ICommand Favorite { get { return _favorite; } }
    }
}
