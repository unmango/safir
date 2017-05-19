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

            modelBuilder.Entity<Playlist>().ToTable("Playlist");

            modelBuilder.Entity<Artist>().ToTable("Artist");
            modelBuilder.Entity<Artist>().HasMany(x => x.Albums);
            modelBuilder.Entity<Artist>().HasMany(x => x.Songs); //TODO:

            modelBuilder.Entity<Album>().ToTable("Album");
            modelBuilder.Entity<Album>().HasMany(x => x.Songs);
            modelBuilder.Entity<Album>().HasOptional(x => x.PrimaryArtist); //TODO:

            modelBuilder.Entity<Song>().ToTable("Song");
            modelBuilder.Entity<Song>().HasOptional(x => x.Album); //TODO:
        }
    }
}
