using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.Manager.Core
{
    public class Artist
    {
        public virtual List<Album> Albums { get; set; }
        public virtual List<Song> Songs { get; set; }
    }
}
