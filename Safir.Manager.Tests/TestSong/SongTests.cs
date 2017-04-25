using Safir.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Safir.Manager.Tests.TestSong
{
    public class SongFixture : IDisposable
    {
        private const string resourceDir = "SongResources";
        private const string fileName = "song.mp3";
        private const int waitm = 200;

        private bool cleanup = false;

        public Uri validFilePath;

        public SongFixture()
        {
            var dir = Path.GetFullPath(resourceDir);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            validFilePath = new Uri(Path.Combine(dir, fileName));

            if (!File.Exists(validFilePath.AbsolutePath))
            {
                File.Create(validFilePath.AbsolutePath);
                Thread.Sleep(waitm);
            }
        }

        public void Dispose()
        {
            var dir = Path.GetFullPath(resourceDir);

            if (cleanup)
                foreach (var file in Directory.EnumerateFiles(dir))
                {
                    File.Delete(file);
                }
        }
    }

    public class SongTests : IClassFixture<SongFixture>, IDisposable
    {
        SongFixture song;

        public SongTests(SongFixture data)
        {
            song = data;
        }

        [Fact]
        public void Constructor_ValidInput()
        {
            var testSong = new Song(song.validFilePath);
        }

        [Fact]
        public void ListProperty_Add()
        {
            var testSong = new Song(song.validFilePath);
            testSong.Artists.Add("testArtist");
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
