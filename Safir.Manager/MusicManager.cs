using Safir.Manager.Core;
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

        public MusicManager(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _context = new MusicContext(connectionString);
        }

        public IEnumerable<Song> GetAllSongs()
        {
            using (var w = new UnitOfWork(_context))
                return w.SongRepository.Get();
        }

        public IEnumerable<Album> GetAllAlbums()
        {
            using (var w = new UnitOfWork(_context))
                return w.AlbumRepository.Get();
        }

        public void AddSongs(IEnumerable<Song> songs)
        {
            using (var w = new UnitOfWork(_context))
            {
                foreach (var song in songs)
                    w.SongRepository.Insert(song);
                w.Save();
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
