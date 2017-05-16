using Safir.Manager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.Manager
{
    public class UnitOfWork : IDisposable
    {
        private readonly MusicContext _context;
        private Repository<Album> _albumRepository;
        private Repository<Song> _songRepository;

        public UnitOfWork(MusicContext context)
        {
            _context = context;
        }

        public Repository<Album> AlbumRepository
        {
            get
            {
                if (_albumRepository == null)
                    _albumRepository = new Repository<Album>(_context);
                return _albumRepository;
            }
        }

        public Repository<Song> SongRepository
        {
            get
            {
                if (_songRepository == null)
                    _songRepository = new Repository<Song>(_context);
                return _songRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
