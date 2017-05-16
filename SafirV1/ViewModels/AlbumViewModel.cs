using Safir.Manager.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Safir.ViewModels
{
    internal class AlbumViewModel
    {
        public AlbumViewModel()
        {
#if DEBUG
            Songs = new List<Song>
            {
                //new Song(new Uri(Path.Combine(Path.GetTempPath(), @"Resources\Debug\test.mp3"))),
                new Song(),
            };
#endif
        }

        public List<Song> Songs { get; set; }

        public string ImagePath
        {
            get
            {
#if DEBUG
                return "../Resources/Design/temp.ico";
#endif
            }
        }
    }
}
