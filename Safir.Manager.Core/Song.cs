using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.Core
{
    public class Song
    {
        #region Private Fields

        #endregion

        #region Constructors

        #endregion

        #region Public Properties

        public virtual Guid SongId { get; set; }

        public virtual Uri FilePath { get; set; }

        public virtual string Title { get; set; }
        public virtual Album Album { get; set; }
        public virtual ICollection<Artist> Artists { get; set; }
        public virtual ICollection<Artist> AlbumArtists { get; set; }
        public virtual ICollection<string> Genres { get; set; }
        public virtual ICollection<string> Composers { get; set; }
        public virtual TimeSpan Duration { get; set; }
        public virtual uint? Year { get; set; }
        public virtual DateTime? ReleaseDate { get; set; }
        public virtual uint? Track { get; set; }
        public virtual uint? Disk { get; set; }
        public virtual uint? DiscCount { get; set; }
        public virtual string Grouping { get; set; }
        public virtual bool? Compilation { get; set; }
        public virtual string Lyrics { get; set; }
        public virtual string Comments { get; set; }
        public virtual uint? BeatsPerMinute { get; set; }

        public virtual string MusicBrainzArtistId { get; set; }
        public virtual string MusicBrainzReleaseId { get; set; }
        public virtual string MusicBrainzReleaseArtistId { get; set; }
        public virtual string MusicBrainzTrackId { get; set; }
        public virtual string MusicBrainzDiscId { get; set; }
        public virtual string MusicBrainzReleaseStatus { get; set; }
        public virtual string MusicBrainzReleaseType { get; set; }
        public virtual string MusicBrainzReleaseCountry { get; set; }

        public virtual uint? Rating { get; set; }
        public virtual ulong PlayCount { get; set; }
        public virtual DateTime DateAdded { get; set; }
        public virtual DateTime DateModified { get; set; }

        #endregion

        #region Public Methods

        #endregion
    }
}
