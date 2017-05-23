using MimeSharp;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Safir.Core
{
    using Helpers;
    using Popularimeter;

    public class BoundSong
    {
        #region Private Fields

        private bool _bound = false;
        private bool _brokenBinding = false;

        private TagLib.File _song;
        private IPopularimeter _popMeter;

        private string _title;
        private List<string> _artists;
        private string _album;
        private List<string> _albumArtists;
        private List<string> _genres;
        private List<string> _composers;
        private TimeSpan _duration;
        private uint? _year;
        private DateTime _releaseDate;
        private uint? _track;
        private uint? _diskNo;
        private bool? _compilation;
        private string _lyrics;
        private int? _rating;
        private ulong? _playCount;
        private string _comments;

        private Func<object, bool> _filePriority; //TODO: Rename and implement in file

        #endregion

        #region Constructors

        public BoundSong()
        {
            if (FilePath != null)
                try { Bind(FilePath); }
                catch (Exception) { _brokenBinding = true; }

            // Default is DB priority
            _filePriority = (e) => (_bound && e == null);
        }

        #endregion

        #region Public Properties

        public virtual Uri FilePath { get; set; }

        public string Title
        {
            get
            {
                if (_filePriority(_title)) // First usage of priority functon
                    _title = _song.Tag.Title;
                return _title;
            }

            set
            {
                _title = value;
                if (_bound)
                    _song.Tag.Title = _title;
            }
        }

        public List<string> Artists
        {
            get
            {
                if (_bound && _artists == null)
                    _artists = new List<string>(_song.Tag.Performers);
                return _artists;
            }

            set
            {
                _artists = value;
                if (_bound)
                    _song.Tag.Performers = _artists.ToArray();
            }
        }

        public string Album
        {
            get
            {
                if (_bound && _album == null)
                    _album = _song.Tag.Album;
                return _album;
            }

            set
            {
                _album = value;
                if (_bound)
                    _song.Tag.Album = _album;
            }
        }

        public List<string> AlbumArtists
        {
            get
            {
                if (_bound && _albumArtists == null)
                    _albumArtists = new List<string>(_song.Tag.AlbumArtists);
                return _albumArtists;
            }

            set
            {
                _albumArtists = value;
                if (_bound)
                    _song.Tag.AlbumArtists = _albumArtists.ToArray();
            }
        }

        public List<string> Genres
        {
            get
            {
                if (_bound && _genres == null)
                    _genres = new List<string>(_song.Tag.Genres);
                return _genres;
            }

            set
            {
                _genres = value;
                if (_bound)
                    _song.Tag.Genres = _genres.ToArray();
            }
        }

        public List<string> Composers
        {
            get
            {
                if (_bound && _composers == null)
                    _composers = new List<string>(_song.Tag.Composers);
                return _composers;
            }

            set
            {
                _composers = value;
                if (_bound)
                    _song.Tag.Composers = _composers.ToArray();
            }
        }

        public TimeSpan Duration
        {
            get
            {
                if (_bound) //TODO: Could later add support for modifying song duration
                    _duration = _song.Properties.Duration;
                return _duration;
            }
            set
            {
                _duration = value;
            }
        }

        public uint Year
        {
            get
            {
                if (_bound && _year == null)
                    _year = _song.Tag.Year;
                return (uint)_year;
            }

            set
            {
                _year = value;
                if (_bound)
                    _song.Tag.Year = (uint)_year;
            }
        }

        public DateTime ReleaseDate
        {
            get
            {
                if (_releaseDate == null)
                    _releaseDate = new DateTime((int)Year, 0, 0); //TODO: 
                var dateString = $"{Year}";
                return DateTime.Parse(dateString);
            }

            set
            {
                _releaseDate = value;
                if (_bound)
                    _song.Tag.Year = Convert.ToUInt32(_releaseDate.Year);
            }
        }

        public uint Track
        {
            get
            {
                if (_bound && _track == null)
                    _track = _song.Tag.Track;
                return (uint)_track;
            }

            set
            {
                _track = value;
                if (_bound)
                    _song.Tag.Track = (uint)_track;
            }
        }

        public uint DiskNo
        {
            get
            {
                if (_bound && _diskNo == null)
                    _diskNo = _song.Tag.Disc;
                return (uint)_diskNo;
            }

            set
            {
                _diskNo = value;
                if (_bound)
                    _song.Tag.Disc = (uint)_diskNo;
            }
        }

        public bool Compilation
        {
            get
            {
                if (_bound && _compilation == null)
                    _compilation = _song.Tag.Performers.Length > 1;
                return (bool)_compilation;
            }
            set
            {
                _compilation = value;
            }
        }

        public string Lyrics
        {
            get
            {
                if (_bound && _lyrics == null)
                    _lyrics = _song.Tag.Lyrics;
                return _lyrics;
            }

            set
            {
                _lyrics = value;
                if (_bound)
                    _song.Tag.Lyrics = _lyrics;
            }
        }

        public int Rating
        {
            get
            {
                //return popularimeter.Rating;
                return (int)_rating;
            }

            set
            {
                _rating = value;
                //var bytes = BitConverter.GetBytes(value);
                //if (bytes.Length > 1)
                //    throw new ArgumentException("Value is too large");
                //popularimeter.Rating = bytes[0];
            }
        }

        public ulong PlayCount
        {
            get
            {
                //return popularimeter.PlayCount;
                return (ulong)_playCount;
            }
            set
            {
                _playCount = value;
            }
        }

        public string Comments
        {
            get
            {
                if (_bound && _comments == null)
                    _comments = _song.Tag.Comment;
                return _comments;
            }

            set
            {
                _comments = value;
                if (_bound)
                    _song.Tag.Comment = _comments;
            }
        }

        #endregion

        #region Public Methods

        public void Bind(Uri filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            if (!filePath.IsFile || !File.Exists(filePath.AbsolutePath))
                throw new ArgumentException("Not a file", nameof(filePath));

            FilePath = filePath;
            _song = TagLib.File.Create(filePath.AbsolutePath);
            //_rating = Popularity.Get(_song);

            _bound = true;
        }

        public bool IsBound()
        {
            return _bound && FilePath != null && !_brokenBinding;
        }

        public void Save()
        {
            if (!_bound)
                return;
            _song.Tag.Performers = _artists.ToArray();
            _song.Tag.AlbumArtists = _artists.ToArray();
            _song.Tag.Genres = _genres.ToArray();
            _song.Tag.Composers = _composers.ToArray();

            _song.Save();
        }

        #endregion
    }
}
