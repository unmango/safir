using Safir.Manager.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Safir.Manager.Core.Tests.TestSong
{
    public class SongFixture : IDisposable
    {
        private const string resourceDir = "SongResources";
        private const string fileName = "song.mp3";
        private const string flacName = "song.flac";
        private const string textName = "text.txt";
        private const int waitm = 200;

        private bool cleanup = false;

        public Uri validFilePath;
        public Uri invalidFilePath;

        public Uri flacFilePath;

        public SongFixture()
        {
            var dir = Path.GetFullPath(resourceDir);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            validFilePath = new Uri(Path.Combine(dir, fileName));
            invalidFilePath = new Uri(Path.Combine(dir, textName));

            flacFilePath = new Uri(Path.Combine(dir, flacName));
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
        public void Bind_ValidInput()
        {
            var testSong = new Song();
            testSong.Bind(song.validFilePath);
        }

        [Fact]
        [Trait("Song", "Constructor")]
        public void Bind_DirectoryInput()
        {
            var dirPath = new Uri(Path.GetFullPath("JustADir"));
            var testSong = new Song();
            Exception ex = Assert.Throws<ArgumentException>(() => testSong.Bind(dirPath));

            Assert.Equal("Not a file", ex.Message);
        }

        [Fact]
        [Trait("Song", "Constructor")]
        public void Bind_NullInput()
        {
            var testSong = new Song();
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => testSong.Bind(null));

            Assert.Equal("filePath", ex.ParamName);
        }

        [Fact]
        [Trait("Song", "Constructor")]
        public void Bind_NotSupportedInput()
        {
            var testSong = new Song();
            Exception ex = Assert.Throws<ArgumentException>(() => testSong.Bind(song.invalidFilePath));

            Assert.Equal("Filetype not supported", ex.Message);
        }

        [Fact]
        [Trait("Song", "Property")]
        public void ListProperty_Add()
        {
            var testSong = new Song()
            {
                FilePath = song.validFilePath
            };
            testSong.Artists.Add("testArtist");

            Assert.Equal(2, testSong.Artists.Count);
        }

        [Fact]
        [Trait("Song", "EntryPoint")]
        public void SongEntryPoint()
        {
            var testSong = new Song(song.flacFilePath);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
