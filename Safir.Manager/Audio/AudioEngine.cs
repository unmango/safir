// <copyright file="AudioEngine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager.Audio
{
    using System;
    using CSCore.SoundOut;
    using CSCore.Streams.Effects;
    using Safir.Manager.EventArgs;

    public class AudioEngine
    {
        private float _volume;
        private long _position;

        private IPlayable _currentTrack;

        private ISoundOut _soundOut;
        private ISoundOutManager _soundOutManager;
        private IDeviceManager _deviceManager;

        public AudioEngine(ISoundOutManager soundOutManager, IDeviceManager deviceManager) {
            _soundOutManager = soundOutManager;
            _deviceManager = deviceManager;
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
                if (_currentTrack != null) {
                    OnTrackChanged();
                }
            }
        }

        public Equalizer Equalizer { get; set; }

        #endregion

        public bool Initialize() {
            _soundOut = _soundOutManager.OpenSong(CurrentTrack);
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
                _soundOut.Stop()
            }
        }

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
