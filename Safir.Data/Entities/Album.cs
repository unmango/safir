// <copyright file="Album.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Data.Entities
{
    using System;
    using System.Collections.Generic;

    public class Album
    {
        public virtual Guid AlbumId { get; set; }

        public virtual ICollection<Song> Songs { get; set; }

        public virtual string Title { get; set; }

        public virtual Artist PrimaryArtist { get; set; }

        public virtual ICollection<Artist> FeaturedArtists { get; set; }

        public virtual bool? Compilation { get; set; }

        public virtual uint? NumTracks { get; set; }

        public virtual TimeSpan Duration { get; set; }

        public virtual uint? Year { get; set; }

        public virtual DateTime ReleaseDate { get; set; }

        public virtual string PrimaryGenre { get; set; }

        public virtual ICollection<string> Genres { get; set; }

        public virtual uint? Rating { get; set; }

        public virtual uint Disc { get; set; }

        public virtual string Comments { get; set; }

        public virtual Uri Image { get; set; }
    }
}
