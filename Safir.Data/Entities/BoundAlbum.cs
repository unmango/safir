using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Safir.Data.Entities
{
    public class BoundAlbum
    {
        private bool _bound = false;

        private string _name;
        private string _primaryArtist;
        private IEnumerable<string> _featuredArtists;
        private bool? _compilation;
        private int? _numTracks;
        private TimeSpan _duration;
        private uint? _year;
        private DateTime _releaseDate;
        private IEnumerable<string> _genres;
        private int? _rating;

        public virtual List<BoundSong> Songs { get; set; }

        public string Name
        {
            get
            {
                if (_bound && _name == null)
                {
                    var val = Songs.First().Album;
                    if (Songs.All(s => s.Album == val))
                        _name = val;
                    else
                        throw new NotImplementedException("One or more songs is a different album");
                }
                return _name;
            }
            set
            {
                if (_bound)
                    Songs.ForEach(s => s.Album = value);
                _name = value;
            }
        }
        public string PrimaryArtist
        {
            get
            {
                if (_bound && _primaryArtist == null)
                {
                    var val = new List<string>();
                    Songs.ForEach(s => val.Concat(s.Artists));
                    var g = val.GroupBy(i => i);
                    var max = g.Aggregate((curMax, x) => (curMax == null || x.Count() < curMax.Count() ? x : curMax));
                    _primaryArtist = max.Key;
                }
                return _primaryArtist;
            }
            set
            {
                _primaryArtist = value;
            }
        }
        public IEnumerable<string> FeaturedArtists
        {
            get
            {
                if (_bound && _featuredArtists == null)
                {
                    var val = new List<string>();
                    Songs.ForEach(s => val.Union(s.Artists));
                    _featuredArtists = val;
                }
                return _featuredArtists;
            }
            set
            {
                _featuredArtists = value;
            }
        }
        public bool Compilation
        {
            get
            {
                if (_bound && _compilation == null)
                    _compilation = Songs.Any(s => s.Compilation);
                return (bool)_compilation;
            }
            set
            {
                _compilation = value;
            }
        }
        public int Count
        {
            get
            {
                return _bound && _numTracks == null ? Songs.Count : (int)_numTracks;
            }
            set
            {
                _numTracks = value;
            }
        }
        public TimeSpan Duration
        {
            get
            {
                if (_bound && _duration == null)
                {
                    var spans = Songs.Select(s => s.Duration.TotalMinutes);
                    var sum = spans.Sum();
                    _duration = TimeSpan.FromMinutes(sum);
                }
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
                {
                    var val = Songs.First().Year;
                    if (Songs.All(s => s.Year == val))
                        _year = val;
                    else
                        throw new NotImplementedException("One or more songs has a different year");
                }
                return (uint)_year;
            }
            set
            {
                if (_bound)
                    Songs.ForEach(s => s.Year = value);
                _year = value;
            }
        }
        public DateTime ReleaseDate
        {
            get
            {
                if (_bound && _releaseDate == null)
                {
                    var val = Songs.First().ReleaseDate;
                    if (Songs.All(s => s.ReleaseDate == val))
                        _releaseDate = val;
                    else
                        throw new NotImplementedException("One or more songs has a different release date");
                }
                return _releaseDate;
            }
            set
            {
                _releaseDate = value;
            }
        }
        public IEnumerable<string> Genres
        {
            get
            {
                if (_bound && _genres == null)
                {
                    var val = new List<string>();
                    Songs.ForEach(s => val.Union(s.Genres));
                    _genres = val;
                }
                return _genres;
            }
            set
            {
                _genres = value;
            }
        }
        public int DiskNo { get; set; }
        public int Rating
        {
            get
            {
                if (_bound && _rating == null)
                {
                    throw new NotImplementedException("Add logic for determining rating based on songs. Could return simple average");
                }
                return (int)_rating;
            }
            set
            {
                _rating = value;
            }
        }
        public string Comments { get; set; }

        public bool IsBound => _bound;

        public void Bind(IEnumerable<BoundSong> songs)
        {
            Songs = songs?.ToList() ?? throw new ArgumentNullException(nameof(songs));
            _bound = true;
        }
    }
}
