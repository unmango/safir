namespace Safir.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Annotations;
    using Caliburn.Micro;
    using Events;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class PlaybackViewModel :
        INotifyPropertyChanged,
        IHandle<PlaySongEvent>,
        IHandle<PauseSongEvent>,
        IHandle<NextSongEvent>,
        IHandle<PreviousSongEvent>
    {
        public PlaybackViewModel(
            IEventAggregator eventAggregator) {
            eventAggregator.Subscribe(this);
        }

        public void Handle(PlaySongEvent message) {
            throw new NotImplementedException();
        }

        public void Handle(PauseSongEvent message) {
            throw new NotImplementedException();
        }

        public void Handle(NextSongEvent message) {
            throw new NotImplementedException();
        }

        public void Handle(PreviousSongEvent message) {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
