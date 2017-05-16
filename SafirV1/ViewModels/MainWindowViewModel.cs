using GalaSoft.MvvmLight;
using PropertyChanged;
using Safir.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Safir.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private const string _windowTitleDefault = "Safir";

        private ICommand _play = new PlayCommand();
        private ICommand _pause = new PauseCommand();
        private ICommand _rewind = new RewindCommand();
        private ICommand _fastForward = new FastForwardCommand();
        private ICommand _favorite = new FavoriteCommand();

        public MainWindowViewModel()
        {
#if DEBUG
            CurrMediaView = new LibraryViewModel();
#endif
        }

        public string WindowTitle { get; set; } = _windowTitleDefault;

        public LibraryMenuViewModel LibraryMenuVM { get; set; }
        public PlaylistMenuViewModel PlaylistMenuVM { get; set; }

        public ICommand Play { get { return _play; } }
        public ICommand Pause { get { return _pause; } }
        public ICommand Rewind { get { return _rewind; } }
        public ICommand FastForward { get { return _fastForward; } }
        public ICommand Favorite { get { return _favorite; } }

        public IMediaView CurrMediaView { get; set; }
    }
}
