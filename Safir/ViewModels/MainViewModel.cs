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

    public class MainViewModel : PropertyChangedBase
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
            MainMenuViewModel mainMenuViewModel) {
            //_log = logger;
            AppName = appMeta.AppName;
            Version = appMeta.AppName + " v" + appMeta.AppVersion;
            _settings = settings;
            _settings.Load();
            MainMenuViewModel = mainMenuViewModel;
        }

        #region Window Settings

        public double WindowHeight {
            get {
                var height = _settings.Get(nameof(WindowHeight));
                if (string.IsNullOrEmpty(height) || height.Equals("NaN")) {
                    var newHeight = SystemParameters.MaximizedPrimaryScreenHeight.ToString();
                    _settings.Set(nameof(WindowHeight), newHeight);
                    height = newHeight;
                }
                return double.Parse(height);
            }
            set {
                _settings.Set(nameof(WindowHeight), value.ToString());
            }
        }

        public double WindowWidth {
            get {
                var width = _settings.Get(nameof(WindowWidth));
                if (string.IsNullOrEmpty(width) || width.Equals("NaN")) {
                    var newWidth = SystemParameters.MaximizedPrimaryScreenWidth.ToString();
                    _settings.Set(nameof(WindowWidth), newWidth);
                    width = newWidth;
                }
                return double.Parse(width);
            }
            set {
                _settings.Set(nameof(WindowWidth), value.ToString());
            }
        }

        public string WindowState {
            get {
                var location = _settings.Get(nameof(WindowState));
                if (string.IsNullOrEmpty(location)) {
                    var newLocation = DefaultValue.Get(nameof(WindowState));
                    _settings.Set(nameof(WindowState), newLocation);
                    location = newLocation;
                }
                return location;
            }
            set {
                _settings.Set(nameof(WindowState), value);
            }
        }

        #endregion

        public string AppName { get; private set; }
        public string Version { get; private set; }

        public MainMenuViewModel MainMenuViewModel { get; set; }

        public IContentDisplay Content { get; private set; }



        public ICommand Play { get { return _play; } }
        public ICommand Pause { get { return _pause; } }
        public ICommand Rewind { get { return _rewind; } }
        public ICommand FastForward { get { return _fastForward; } }
        public ICommand Favorite { get { return _favorite; } }

        public ICommand WindowClosing {
            get {
                _settings.Save();
                return null;
            }
        }
    }
}
