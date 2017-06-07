using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.Data.Entities
{
    public class Playlist
    {
        public enum PlaylistType
        {
            Music,
            Video
        }

        public virtual PlaylistType Type { get; set; }
        public virtual Uri Image { get; set; }
        public virtual string Name { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<Artist> Artists { get; set; }
    }
}
