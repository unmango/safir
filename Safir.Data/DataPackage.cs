// <copyright file="DataPackage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Data
{
    using System.Data.Entity.Infrastructure;
    using Entities;
    using Entities.Repositories;
    using Mehdime.Entity;
    using SimpleInjector;

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
