using System;
using System.Collections.Generic;
using System.Text;

namespace Safir.Manager
{
    public class Song
    {
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string AlbumArtist { get; set; }
        public string Genre { get; set; }
        public string Composer { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int TrackNo { get; set; }
        public int DiskNo { get; set; }
        public bool Compilation { get; set; }
        public string Lyrics { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
    }
}
