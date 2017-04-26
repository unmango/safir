using MimeSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Safir.Manager
{
    public class Song
    {
        #region Private Fields

        private TagLib.File _song;

        private List<string> _artists;

        private List<string> _albumArtists;

        private List<string> _genres;

        private List<string> _composers;

        private DateTime _releaseDate;

        #endregion

        #region Constructors

        public Song(Uri filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException("filePath");

            if (!filePath.IsFile || !File.Exists(filePath.AbsolutePath))
                throw new ArgumentException("Not a file");

            if (!FileExtension.Valid(filePath))
                throw new ArgumentException("Filetype not supported");

            _song = TagLib.File.Create(filePath.AbsolutePath);
        }

        #endregion

        #region Public Properties

        public string Title
        {
            get
            {
                return _song.Tag.Title;
            }

            set
            {
                _song.Tag.Title = value;
            }
        }

        public List<string> Artists
        {
            get
            {
                if (_artists == null)
                    _artists = new List<string>(_song.Tag.Performers);
                return _artists;
            }

            set
            {
                _artists = value;
                _song.Tag.Performers = _artists.ToArray();
            }
        }

        public string Album
        {
            get
            {
                return _song.Tag.Album;
            }

            set
            {
                _song.Tag.Album = value;
            }
        }

        public List<string> AlbumArtists
        {
            get
            {
                if (_albumArtists == null)
                    _albumArtists = new List<string>(_song.Tag.AlbumArtists);
                return _albumArtists;
            }

            set
            {
                _albumArtists = value;
                _song.Tag.AlbumArtists = _albumArtists.ToArray();
            }
        }

        public List<string> Genres
        {
            get
            {
                if (_genres == null)
                    _genres = new List<string>(_song.Tag.Genres);
                return _genres;
            }

            set
            {
                _genres = value;
                _song.Tag.Genres = _genres.ToArray();
            }
        }

        public List<string> Composers
        {
            get
            {
                if (_composers == null)
                    _composers = new List<string>(_song.Tag.Composers);
                return _composers;
            }

            set
            {
                _composers = value;
                _song.Tag.Composers = _composers.ToArray();
            }
        }

        public TimeSpan Duration
        {
            get
            {
                return _song.Properties.Duration;
            }
        }

        public uint Year
        {
            get
            {
                return _song.Tag.Year;
            }

            set
            {
                _song.Tag.Year = value;
            }
        }

        public DateTime ReleaseDate
        {
            get
            {
                if (_releaseDate == null)
                    _releaseDate = new DateTime((int)Year, 0, 0);
                var dateString = $"{Year}";
                return DateTime.Parse(dateString);
            }

            set
            {
                _song.Tag.Year = Convert.ToUInt32(value.Year);
            }
        }

        public uint Track
        {
            get
            {
                return _song.Tag.Track;
            }

            set
            {
                _song.Tag.Track = value;
            }
        }

        public uint DiskNo
        {
            get
            {
                return _song.Tag.Disc;
            }

            set
            {
                _song.Tag.Disc = value;
            }
        }

        public bool Compilation
        {
            get
            {
                return _song.Tag.Performers.Length > 1;
            }
        }

        public string Lyrics
        {
            get
            {
                return _song.Tag.Lyrics;
            }

            set
            {
                _song.Tag.Lyrics = value;
            }
        }

        //public int Rating
        //{
        //    get
        //    {
        //        return popularimeter.Rating;
        //    }

        //    set
        //    {
        //        var bytes = BitConverter.GetBytes(value);
        //        if (bytes.Length > 1)
        //            throw new ArgumentException("Value is too large");
        //        popularimeter.Rating = bytes[0];
        //    }
        //}

        //public ulong PlayCount
        //{
        //    get
        //    {
        //        return popularimeter.PlayCount;
        //    }
        //}

        public string Comments
        {
            get
            {
                return _song.Tag.Comment;
            }

            set
            {
                _song.Tag.Comment = value;
            }
        }

        #endregion

        #region Public Methods

        public void Save()
        {
            _song.Tag.Performers = _artists.ToArray();
            _song.Tag.AlbumArtists = _artists.ToArray();
            _song.Tag.Genres = _genres.ToArray();
            _song.Tag.Composers = _composers.ToArray();

            _song.Save();
        }

        #endregion
    }
}
