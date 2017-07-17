// <copyright file="WaveSourceManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager.Audio
{
    using System;
    using CSCore;
    using CSCore.Codecs;

    public class WaveSourceManager : IWaveSourceManager
    {
        public WaveSourceManager() {
        }

        public bool Mono { get; set; } = false;

        public void Dispose() {
            throw new NotImplementedException();
        }

        public IWaveSource GetWaveSource(IPlayable song) {
            IWaveSource waveSource = CodecFactory.Instance.GetCodec(song.FilePath);

            if (Mono) {
                waveSource = waveSource.ToMono();
            }

            return waveSource;
        }
    }
}
