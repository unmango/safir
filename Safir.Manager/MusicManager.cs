using Safir.Core;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.Manager
{
    public class MusicManager
    {
        public enum AddMode
        {
            Scan,
            ScanAndCopy,
            Move
        }

        private readonly MusicContext _context;
        private readonly Container _container;

        public MusicManager(string connectionString, Container container)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _context = new MusicContext(connectionString);
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public IEnumerable<Song> GetAllSongs()
        {
            using (var u = _container.GetInstance<IUnitOfWork>())
                return u.SongRepository.Get();
        }

        public IEnumerable<Album> GetAllAlbums()
        {
            using (var u = new UnitOfWork(_context))
                return u.AlbumRepository.Get();
        }

        public void AddSongs(IEnumerable<Song> songs)
        {
            using (var u = new UnitOfWork(_context))
            {
                foreach (var song in songs)
                    u.SongRepository.Insert(song);
                u.Save();
            }
        }

        public void ScanDirectory(Uri directory, AddMode mode = AddMode.Scan)
        {
            if (!Directory.Exists(directory.AbsolutePath))
                throw new ArgumentException("Not a directory", nameof(directory));

            throw new NotImplementedException();
        }

        public void AddDirectory(Uri directory, AddMode mode = AddMode.Scan)
        {
            ScanDirectory(directory, mode);

            throw new NotImplementedException();
        }

        public void RemoveSongs(IEnumerable<Song> songs)
        {
            throw new NotImplementedException();
        }

        public void RemoveAndDelete(IEnumerable<Song> songs)
        {
            throw new NotImplementedException();
        }
    }
}
