using Mehdime.Entity;
using SimpleInjector;

namespace Safir.Manager
{
    using Core;

    public static class ManagerPackage
    {
        public static void RegisterServices(Container container)
        {
            container.Register<IDbContextScopeFactory>(() => new DbContextScopeFactory());
            container.Register<IAmbientDbContextLocator, AmbientDbContextLocator>();
            container.Register<IRepository<Song>, SongRepository>();
            container.Register<IRepository<Album>, AlbumRepository>();
            container.Register<IRepository<Artist>, ArtistRepository>();
            container.Register<IRepository<Playlist>, PlaylistRepository>();
        }
    }
}
