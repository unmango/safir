// <copyright file="AudioEngine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager.Audio
{
    using System;
    using CSCore.SoundOut;
    using CSCore.Streams.Effects;
    using EventArgs;

    public class AudioEngine : IDisposable
    {
        private IPlayable _currentTrack;

        private ISoundOut _soundOut;
        private readonly ISoundOutManager _soundOutManager;

        public AudioEngine(ISoundOutManager soundOutManager) {
            _soundOutManager = soundOutManager;
            _soundOut = _soundOutManager.OpenSong(null);
        }

        public event EventHandler TrackChanged;

        public event EventHandler TrackFinished;

        #region Public Properties

        public float Volume { get; set; }

        public long Position { get; set; }

        public Equalizer Equalizer { get; set; }

        public bool IsPlaying =>
            (_soundOut != null) &&
            (_soundOut.PlaybackState == PlaybackState.Playing);

        public PlaybackState CurrentState =>
            _soundOut?.PlaybackState ?? PlaybackState.Stopped;

        public IPlayable CurrentTrack {
            get => _currentTrack;
            protected set {
                _currentTrack = value;
                if (_currentTrack != null) {
                    OnTrackChanged();
                }
            }
        }

        #endregion

        #region Methods

        public bool Initialize() {
            return true;
        }

        public void OpenSong(IPlayable song) {
            _soundOut = _soundOutManager.OpenSong(song);
        }

        public void Play() {
            _soundOut?.Play();
        }

        public void Pause() {
            _soundOut?.Pause();
        }

        public void Stop() {
            _soundOut?.Stop();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing) {
            if (!disposing) return;
            _soundOutManager.Dispose();
            _soundOut?.Dispose();
        }

        #endregion

        #region Events

        protected void OnTrackChanged() {
            TrackChanged?.Invoke(this, new TrackChangedEventArgs(CurrentTrack));
        }

        protected void OnTrackFinished() {
            TrackFinished?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
