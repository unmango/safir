// <copyright file="DataPackage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Data
{
    using System.Data.Entity.Infrastructure;
    using Entities.Repositories;
    using Mehdime.Entity;
    using SimpleInjector;

    public static class DataPackage
    {
        public static void RegisterServices(Container container) {
            container.RegisterSingleton<IDbConnectionFactory, SQLiteConnectionFactory>();
            container.RegisterSingleton<SQLiteConfiguration>();

            container.RegisterSingleton<DatabaseManager>();

            //container.Register(() => new MusicContext(ConnectionStringHelper.Get()), Lifestyle.Scoped);
            container.RegisterSingleton<IDbContextFactory, DbContextFactory>();
            container.RegisterSingleton<IDbContextScopeFactory, DbContextScopeFactory>();
            container.RegisterSingleton<IAmbientDbContextLocator, AmbientDbContextLocator>();

            container.RegisterSingleton<SongRepository>();
            container.RegisterSingleton<AlbumRepository>();
            container.RegisterSingleton<ArtistRepository>();
            container.RegisterSingleton<PlaylistRepository>();
        }
    }
}
