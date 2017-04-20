using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Safir.Manager
{
    public class Album
    {
        public List<Song> Songs { get; set; }
        public string Artist { get; set; }
        public bool Compilation { get; set; }
        public int Count => Songs.Count;
        public TimeSpan Duration
        {
            get
            {
                var spans = Songs.Select(s => s.Duration.TotalMinutes);
                var sum = spans.Sum();
                return TimeSpan.FromMinutes(sum);
            }
        }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public int DiskNo { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
    }
}
