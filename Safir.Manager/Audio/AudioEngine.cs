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
        private float _volume;
        private long _position;

        private IPlayable _currentTrack;

        private ISoundOut _soundOut;
        private ISoundOutManager _soundOutManager;

        public AudioEngine(ISoundOutManager soundOutManager) {
            _soundOutManager = soundOutManager;
        }

        public event EventHandler TrackChanged;

        public event EventHandler TrackFinished;

        #region Public Properties

        public float Volume {
            get { return _volume; }
            set { _volume = value; }
        }

        public long Position {
            get { return _position; }
            set { _position = value; }
        }

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

        public Equalizer Equalizer { get; set; }

        #endregion

        #region Methods

        public bool Initialize() {
            return true;
        }

        public void Play() {
            if (_soundOut != null) {
                _soundOut.Play();
            }
        }

        public void Pause() {
            if (_soundOut != null) {
                _soundOut.Pause();
            }
        }

        public void Stop() {
            if (_soundOut != null) {
                _soundOut.Stop();
            }
        }
        
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing) {
            if (disposing) {
                _soundOutManager.Dispose();

                if (_soundOut != null) {
                    _soundOut.Dispose();
                }
            }
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
