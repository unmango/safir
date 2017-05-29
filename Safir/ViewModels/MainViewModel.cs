using GalaSoft.MvvmLight;
using System.Windows.Input;

namespace Safir.ViewModels
{
    using Commands;

    internal class MainViewModel : ViewModelBase
    {
        private ICommand _play = new PlayCommand();
        private ICommand _pause = new PauseCommand();
        private ICommand _rewind = new RewindCommand();
        private ICommand _fastForward = new FastForwardCommand();
        private ICommand _favorite = new FavoriteCommand();

        public MainViewModel()
        {
        }
        
        public ICommand Play { get { return _play; } }
        public ICommand Pause { get { return _pause; } }
        public ICommand Rewind { get { return _rewind; } }
        public ICommand FastForward { get { return _fastForward; } }
        public ICommand Favorite { get { return _favorite; } }
    }
}
