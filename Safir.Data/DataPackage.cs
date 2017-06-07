using Mehdime.Entity;
using SimpleInjector;
using System.Data.Entity.Infrastructure;

namespace Safir.Data
{
    using Entities;
    using Entities.Repositories;

    public static class DataPackage
    {
        public static void RegisterServices(Container container) {
            container.Register<IDbConnectionFactory, SQLiteConnectionFactory>();
            container.Register<SQLiteConfiguration>();

            container.Register<DatabaseManager>();

            container.Register<IDbContextFactory, DbContextFactory>();
            container.Register<IDbContextScopeFactory, DbContextScopeFactory>();
            container.Register<IAmbientDbContextLocator, AmbientDbContextLocator>();

            container.Register<IRepository<Song>, SongRepository>();
            container.Register<IRepository<Album>, AlbumRepository>();
            container.Register<IRepository<Artist>, ArtistRepository>();
            container.Register<IRepository<Playlist>, PlaylistRepository>();
        }
    }
}
