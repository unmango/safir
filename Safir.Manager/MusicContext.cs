using Safir.Core;
using SQLite.CodeFirst;
using System.Data.Entity;

namespace Safir.Manager
{
    public class MusicContext : DbContext, IDbContext
    {
        public MusicContext(string connectionString)
            : base(connectionString)
        { }

        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
#if DEBUG
            var sqliteConnectionInitializer = new SqliteDropCreateDatabaseWhenModelChanges<MusicContext>(modelBuilder);
#else
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<MusicContext>(modelBuilder);
#endif
            Database.SetInitializer(sqliteConnectionInitializer);

            modelBuilder.Entity<Playlist>()
                .HasMany(x => x.Songs);
            modelBuilder.Entity<Playlist>()
                .HasMany(x => x.Albums);
            modelBuilder.Entity<Playlist>()
                .HasMany(x => x.Artists);
            modelBuilder.Entity<Playlist>()
                .HasKey(x => x.Name)
                .ToTable("Playlists");

            modelBuilder.Entity<Artist>()
                .HasMany(x => x.Albums)
                .WithMany(x => x.FeaturedArtists);
            modelBuilder.Entity<Artist>()
                .HasMany(x => x.Songs)
                .WithMany(x => x.Artists);
            modelBuilder.Entity<Artist>()
                .HasKey(x => x.ArtistId)
                .ToTable("Artists");

            modelBuilder.Entity<Album>()
                .HasMany(x => x.Songs)
                .WithOptional(x => x.Album);
            modelBuilder.Entity<Album>()
                .HasMany(x => x.FeaturedArtists)
                .WithMany(x => x.Albums);
            modelBuilder.Entity<Album>()
                .HasKey(x => x.AlbumId)
                .ToTable("Albums")
                .HasOptional(x => x.PrimaryArtist);

            modelBuilder.Entity<Song>()
                .HasOptional(x => x.Album);
            modelBuilder.Entity<Song>()
                .HasMany(x => x.Artists)
                .WithMany(x => x.Songs);
            modelBuilder.Entity<Song>()
                .HasMany(x => x.AlbumArtists)
                .WithMany(x => x.Songs);
            modelBuilder.Entity<Song>()
                .HasKey(x => x.SongId)
                .ToTable("Songs");
        }
    }
}
