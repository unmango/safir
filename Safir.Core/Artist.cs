using System;
using System.Collections.Generic;

namespace Safir.Core
{
    public class Artist
    {
        public virtual Guid ArtistId { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
        public virtual string Name { get; set; }
    }
}
