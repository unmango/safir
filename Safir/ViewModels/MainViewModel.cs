using GalaSoft.MvvmLight;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Safir.ViewModels
{
    using Commands;
    using Core;
    using Manager;

    internal class MainViewModel : ViewModelBase
    {
        IDbContextScopeFactory _factory;

        private ICommand _play = new PlayCommand();
        private ICommand _pause = new PauseCommand();
        private ICommand _rewind = new RewindCommand();
        private ICommand _fastForward = new FastForwardCommand();
        private ICommand _favorite = new FavoriteCommand();

        public MainViewModel(IDbContextScopeFactory factory)
        {
            _factory = factory;
        }
        
        public ICommand Play { get { return _play; } }
        public ICommand Pause { get { return _pause; } }
        public ICommand Rewind { get { return _rewind; } }
        public ICommand FastForward { get { return _fastForward; } }
        public ICommand Favorite { get { return _favorite; } }
    }
}
