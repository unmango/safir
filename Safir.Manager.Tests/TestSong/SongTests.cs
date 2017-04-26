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
        private const string textName = "text.txt";
        private const int waitm = 200;

        private bool cleanup = false;

        public Uri validFilePath;
        public Uri invalidFilePath;

        public SongFixture()
        {
            var dir = Path.GetFullPath(resourceDir);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            validFilePath = new Uri(Path.Combine(dir, fileName));
            invalidFilePath = new Uri(Path.Combine(dir, textName));
        }

        public void Dispose()
        {
            if (cleanup)
            {
                var dir = Path.GetFullPath(resourceDir);

                foreach (var file in Directory.EnumerateFiles(dir))
                {
                    File.Delete(file);
                }
            }
        }
    }

    [Trait("Class", "Song")]
    public class SongTests : IClassFixture<SongFixture>, IDisposable
    {
        SongFixture song;

        public SongTests(SongFixture data)
        {
            song = data;
        }

        [Fact]
        [Trait("Song", "Constructor")]
        public void Constructor_ValidInput()
        {
            var testSong = new Song(song.validFilePath);
        }

        [Fact]
        [Trait("Song", "Constructor")]
        public void Constructor_DirectoryInput()
        {
            var dirPath = new Uri(Path.GetFullPath("JustADir"));
            Exception ex = Assert.Throws<ArgumentException>(() => new Song(dirPath));

            Assert.Equal("Not a file", ex.Message);
        }

        [Fact]
        [Trait("Song", "Constructor")]
        public void Constructor_NullInput()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new Song(null));

            Assert.Equal("filePath", ex.ParamName);
        }

        [Fact]
        [Trait("Song", "Constructor")]
        public void Constructor_NotSupportedInput()
        {
            Exception ex = Assert.Throws<ArgumentException>(() => new Song(song.invalidFilePath));

            Assert.Equal("Filetype not supported", ex.Message);
        }

        [Fact]
        [Trait("Song", "Property")]
        public void ListProperty_Add()
        {
            var testSong = new Song(song.validFilePath);
            testSong.Artists.Add("testArtist");

            Assert.Equal(2, testSong.Artists.Count);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
