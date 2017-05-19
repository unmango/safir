using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.Core
{
    public class Playlist
    {
        public PlaylistType Type { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }

        public virtual List<Song> Songs { get; set; }
        public virtual List<Album> Albums { get; set; }
        public virtual List<Artist> Artists { get; set; }
    }
}
