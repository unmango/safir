using Safir.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.Manager
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _context;
        private readonly Dictionary<Type, IRepository> _repositories;

        public UnitOfWork(IDbContext context)
        {
            _context = context;
        }

        public void Register(IRepository repo)
        {
            _repositories.Add(repo.GetType(), repo);
        }

        #region NonIOC
        
        //private IRepository<Playlist> _playlistRepository;
        //private IRepository<Artist> _artistRepository;
        //private IRepository<Album> _albumRepository;
        //private IRepository<Song> _songRepository;

        //public IRepository<Playlist> PlaylistRepository
        //{
        //    get
        //    {
        //        if (_playlistRepository == null)
        //            _playlistRepository = new Repository<Playlist>(_context);
        //        return _playlistRepository;
        //    }
        //}

        //public IRepository<Artist> ArtistRepository
        //{
        //    get
        //    {
        //        if (_artistRepository == null)
        //            _artistRepository = new Repository<Artist>(_context);
        //        return _artistRepository;
        //    }
        //}

        //public IRepository<Album> AlbumRepository
        //{
        //    get
        //    {
        //        if (_albumRepository == null)
        //            _albumRepository = new Repository<Album>(_context);
        //        return _albumRepository;
        //    }
        //}

        //public IRepository<Song> SongRepository
        //{
        //    get
        //    {
        //        if (_songRepository == null)
        //            _songRepository = new Repository<Song>(_context);
        //        return _songRepository;
        //    }
        //}

        #endregion

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
