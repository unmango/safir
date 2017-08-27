// <copyright file="MusicContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Data
{
    using System.Data.Entity;
    using Entities;
    using SQLite.CodeFirst;

    public class MusicContext : DbContext
    {
        //public MusicContext(string connectionString)
        //    : base(connectionString) {
        //}

        public DbSet<Playlist> Playlists { get; set; }

        public DbSet<Artist> Artists { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Song> Songs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Playlist>()
                .HasKey(x => x.Name)
                .ToTable("Playlists");
            // modelBuilder.Entity<Playlist>()
            //    .HasMany(x => x.Songs);
            // modelBuilder.Entity<Playlist>()
            //    .HasMany(x => x.Albums);
            // modelBuilder.Entity<Playlist>()
            //    .HasMany(x => x.Artists);

            modelBuilder.Entity<Artist>()
                .HasKey(x => x.ArtistId)
                .ToTable("Artists");
            // modelBuilder.Entity<Artist>()
            //    .HasMany(x => x.Albums)
            //    .WithMany(x => x.FeaturedArtists);
            // modelBuilder.Entity<Artist>()
            //    .HasMany(x => x.Songs)
            //    .WithMany(x => x.Artists);

            modelBuilder.Entity<Album>()
                .HasKey(x => x.AlbumId)
                .ToTable("Albums")
                .HasOptional(x => x.PrimaryArtist);
            // modelBuilder.Entity<Album>()
            //    .HasMany(x => x.Songs)
            //    .WithOptional(x => x.Album);
            // modelBuilder.Entity<Album>()
            //    .HasMany(x => x.FeaturedArtists)
            //    .WithMany(x => x.Albums);

            modelBuilder.Entity<Song>()
                .HasKey(x => x.SongId)
                .ToTable("Songs");
            // modelBuilder.Entity<Song>()
            //    .HasOptional(x => x.Album);
            // modelBuilder.Entity<Song>()
            //    .HasMany(x => x.Artists)
            //    .WithMany(x => x.Songs);
            // modelBuilder.Entity<Song>()
            //    .HasMany(x => x.AlbumArtists)
            //    .WithMany(x => x.Songs);

            var sqliteConnectionInitializer =
#if DEBUG
                new SqliteDropCreateDatabaseWhenModelChanges<MusicContext>(modelBuilder);
#else
                new SqliteCreateDatabaseIfNotExists<MusicContext>(modelBuilder);
#endif
            Database.SetInitializer(sqliteConnectionInitializer);
        }
    }
}
