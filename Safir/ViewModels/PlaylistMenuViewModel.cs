using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.ViewModels
{
    using Data.Entities;
    using Data.Entities.Repositories;
    using Core.Settings;

    public class PlaylistMenuViewModel
    {
        private ISettingStore _settings;

        private IRepository<Playlist> _playlists;

        public PlaylistMenuViewModel(
            ISettingStore settings,
            IRepository<Playlist> playlists) {
            _settings = settings;
            _playlists = playlists;
        }


    }
}
