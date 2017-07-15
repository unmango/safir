// <copyright file="PlaySong.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager
{
    using System;
    using CSCore;
    using CSCore.Codecs;
    using CSCore.CoreAudioAPI;
    using CSCore.SoundOut;

    public class PlaySong
    {
        private string _file;
        private ISoundOut _soundOut;
        private IWaveSource _waveSource;

        public PlaySong(string file) {
            _file = file;
        }

        public event EventHandler<PlaybackStoppedEventArgs> PlaybackStopped;

        public void Open(MMDevice device) {
            CleanupPlayback();

            _waveSource =
                CodecFactory.Instance.GetCodec(_file)
                    .ToSampleSource()
                    .ToMono()
                    .ToWaveSource();
            _soundOut = new WasapiOut() { Latency = 100, Device = device };
            _soundOut.Initialize(_waveSource);
            if (PlaybackStopped != null) {
                _soundOut.Stopped += PlaybackStopped;
            }
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

        public void CleanupPlayback() {
            if (_soundOut != null) {
                _soundOut.Dispose();
                _soundOut = null;
            }

            if (_waveSource != null) {
                _waveSource.Dispose();
                _waveSource = null;
            }
        }
    }
}
