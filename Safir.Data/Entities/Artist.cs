// <copyright file="Artist.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Data.Entities
{
    using System;
    using System.Collections.Generic;

    public class Artist
    {
        public virtual Guid ArtistId { get; set; }

        public virtual ICollection<Album> Albums { get; set; }

        public virtual ICollection<Song> Songs { get; set; }

        public virtual string Name { get; set; }
    }
}
