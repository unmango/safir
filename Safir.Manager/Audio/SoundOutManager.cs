// <copyright file="SoundOutManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager.Audio
{
    using System;
    using CSCore.CoreAudioAPI;
    using CSCore.SoundOut;

    public class SoundOutManager : ISoundOutManager
    {
        private IDeviceManager _deviceManager;
        private IWaveSourceManager _waveSourceManager;

        public SoundOutManager(IDeviceManager deviceManager, IWaveSourceManager waveSourceManager) {
            _deviceManager = deviceManager;
            _waveSourceManager = waveSourceManager;
        }

        public int Latency { get; set; } = 100;

        public ISoundOut OpenSong(IPlayable song) {
            ISoundOut soundOut;

            IMMDevice mmDevice = _deviceManager.GetDefaultDevice();

            // TODO: DirectSoundOut support?
            if (WasapiOut.IsSupportedOnCurrentPlatform) {
                soundOut = new WasapiOut() { Latency = Latency, Device = (MMDevice)mmDevice };
            } else {
                soundOut = new WaveOut() { Latency = Latency, Device = (WaveOutDevice)mmDevice };
            }

            var waveSource = _waveSourceManager.GetWaveSource(song);
            soundOut.Initialize(waveSource);

            return soundOut;
        }
        
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing) {
            _deviceManager?.Dispose();
            _waveSourceManager?.Dispose();
        }
    }
}
