using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Safir.Common;
using Safir.Common.Data;
using Safir.Common.Domain;
using Safir.FileManager.Domain.Entities;
using Safir.FileManager.Infrastructure.Data.Configuration;

namespace Safir.FileManager.Infrastructure.Data
{
    public class FileContext : DbContext, IUnitOfWork, IDispatchEvents
    {
        private readonly IOptions<FileManagerOptions> _options;
        private readonly IEventDispatcher _dispatcher;

        public FileContext(
            DbContextOptions<FileContext> contextOptions,
            IOptions<FileManagerOptions> options,
            IEventDispatcher dispatcher)
            : base(contextOptions)
        {
            _options = options;
            _dispatcher = dispatcher;
        }

        // Set DbSet<T>'s to null! because of
        // https://docs.microsoft.com/en-us/ef/core/miscellaneous/nullable-reference-types

        public DbSet<Library> Libraries { get; protected set; } = null!;

        public DbSet<Media> Media { get; protected set; } = null!;

        public DbSet<TrackedFile> TrackedFiles { get; protected set; } = null!;

        public IEnumerable<Entity> GetEntities()
        {
            return ChangeTracker.Entries<Entity>()
                .Select(x => x.Entity)
                .Where(x => x.Events?.Any() == true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (builder.IsConfigured) return;

            builder.UseSqlite(_options.Value.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new MediaConfiguration());
        }

        async ValueTask IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
        {
            await EventHelper.DispatchAsync(this, _dispatcher, cancellationToken);
            await SaveChangesAsync(cancellationToken);
        }
    }
}
