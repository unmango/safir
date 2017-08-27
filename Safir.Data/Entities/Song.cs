// <copyright file="Song.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Data.Entities
{
    using System;
    using System.Collections.Generic;

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

        public static implicit operator Song(TagLib.File file) {
            var tags = file.Tag;
            var props = file.Properties;
            var song = new Song {
                Track = tags.Track,
                Title = tags.Title,
                //Album = tags.Album,
                //AlbumArtists = tags.AlbumArtists,
                //Artists = tags.Artists,
                BeatsPerMinute = tags.BeatsPerMinute,
                Comments = tags.Comment,
                Compilation = tags.AlbumArtists.Length > 1,
                Composers = tags.Composers,
                //DateAdded = null,
                //DateModified = null,
                DiscCount = tags.DiscCount,
                Disk = tags.Disc,
                Duration = props.Duration,
                Genres = tags.Genres,
                Grouping = tags.Grouping,
                Lyrics = tags.Lyrics,
                MusicBrainzArtistId = tags.MusicBrainzArtistId,
                MusicBrainzDiscId = tags.MusicBrainzDiscId,
                MusicBrainzReleaseArtistId = tags.MusicBrainzReleaseArtistId,
                MusicBrainzReleaseCountry = tags.MusicBrainzReleaseCountry,
                MusicBrainzReleaseId = tags.MusicBrainzReleaseId,
                MusicBrainzReleaseStatus = tags.MusicBrainzReleaseStatus,
                MusicBrainzReleaseType = tags.MusicBrainzReleaseType,
                MusicBrainzTrackId = tags.MusicBrainzTrackId,
                PlayCount = 0,
                Rating = 0,
                ReleaseDate = new DateTime((int)tags.Year, 1, 1),
                Year = tags.Year,
                SongId = default(Guid)
            };
            return song;
        }

        #endregion
    }
}
