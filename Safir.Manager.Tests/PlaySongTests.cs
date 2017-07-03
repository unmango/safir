using CSCore.CoreAudioAPI;
using System;
using System.IO;
using Xunit;

namespace Safir.Manager.Tests
{
    public class PlaySongTests
    {
        private readonly string _mp3Song;
        private readonly string _m4aSong;
        private readonly string _flacSong;

        public PlaySongTests() {
            var resourceDir = new DirectoryInfo(Environment.CurrentDirectory + "/Resources");
            foreach (var file in resourceDir.GetFiles()) {
                if (file.Name.EndsWith(".mp3")) {
                    _mp3Song = file.FullName;
                }
                if (file.Name.EndsWith(".m4a")) {
                    _m4aSong = file.FullName;
                }
                if (file.Name.EndsWith(".flac")) {
                    _flacSong = file.FullName;
                }
            }
        }

        [Fact]
        [Trait("Song", "Play")]
        public void PlayMp3Test() {
            var song = new PlaySong(_mp3Song);
            using (var mmdeviceEnumerator = new MMDeviceEnumerator())
            using (var mmdeviceCollection = mmdeviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                song.Open(mmdeviceCollection[0]);
            song.Play();
            song.Stop();
        }

        [Fact]
        [Trait("Song", "Play")]
        public void PlayM4aTest() {
            var song = new PlaySong(_m4aSong);
            using (var mmdeviceEnumerator = new MMDeviceEnumerator())
            using (var mmdeviceCollection = mmdeviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                song.Open(mmdeviceCollection[0]);
            song.Play();
            song.Stop();
        }

        [Fact]
        [Trait("Song", "Play")]
        public void PlayFlacTest() {
            var song = new PlaySong(_flacSong);
            using (var mmdeviceEnumerator = new MMDeviceEnumerator())
            using (var mmdeviceCollection = mmdeviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                song.Open(mmdeviceCollection[0]);
            song.Play();
            song.Stop();
        }
    }
}
