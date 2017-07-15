// <copyright file="AudioEngineTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager.Tests.Audio
{
    using CSCore.SoundOut;
    using Moq;
    using Safir.Manager.Audio;
    using Xunit;

    public class AudioEngineTests
    {
        private Mock<IPlayable> _song;
        private Mock<ISoundOut> _soundOut;
        private Mock<IWaveSourceManager> _waveSourceManager;
        private Mock<ISoundOutManager> _soundOutManager;
        private AudioEngine _engine;

        public AudioEngineTests() {
            _song = new Mock<IPlayable>();
            _soundOut = new Mock<ISoundOut>();
            _waveSourceManager = new Mock<IWaveSourceManager>();
            _soundOutManager = new Mock<ISoundOutManager>();
            _engine = new AudioEngine(_soundOutManager.Object);
        }

        [Fact]
        public void OpenSongTest() {
            _soundOutManager.Setup(x => x.OpenSong(_song.Object)).Returns(_soundOut.Object);
        }
    }
}
