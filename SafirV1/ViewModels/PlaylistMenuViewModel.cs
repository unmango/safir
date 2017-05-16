using GalaSoft.MvvmLight;
using Safir.Manager.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.ViewModels
{
    internal class PlaylistMenuViewModel : ViewModelBase
    {
        public PlaylistMenuViewModel()
        {
#if DEBUG
            Items = new List<Playlist>
            {
                new Playlist { Type = PlaylistType.Music, Image = "../Resources/Icons/brand.ico", Name = "TestItem" },
                new Playlist { Type = PlaylistType.Video, Image = "../Resources/Icons/brand.ico", Name = "TestItem2" },
                new Playlist { Type = PlaylistType.Music, Image = "../Resources/Icons/brand.ico", Name = "TestItem3" },
                new Playlist { Type = PlaylistType.Video, Image = "../Resources/Icons/brand.ico", Name = "TestItem4" },
            };
#endif
        }

        public List<Playlist> Items { get; set; }
        public PlaylistType LastExpanded
        {
            get
            {
                return PlaylistType.Music;
            }
        }
    }
}
