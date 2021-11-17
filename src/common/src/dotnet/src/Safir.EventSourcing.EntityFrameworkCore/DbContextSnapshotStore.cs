using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Safir.EventSourcing.EntityFrameworkCore
{
    [PublicAPI]
    public class DbContextSnapshotStore<TContext> : ISnapshotStore
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly ILogger<DbContextSnapshotStore<TContext>> _logger;

        public DbContextSnapshotStore(TContext context, ILogger<DbContextSnapshotStore<TContext>> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync<TAggregate, TId>(TAggregate aggregate, CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TId>
        {
            _logger.LogDebug("Adding aggregate snapshot");
            await _context.AddAsync(aggregate, cancellationToken);
            _logger.LogTrace("Added aggregate snapshot");

            _logger.LogTrace("Saving context changes");
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogTrace("Saved context changes");
        }

        public Task<TAggregate> FindAsync<TAggregate, TId>(
            TId id,
            int maxVersion = int.MaxValue,
            CancellationToken cancellationToken = default)
            where TAggregate : class, IAggregate<TId>
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            return GetAggregateSet<TAggregate, TId>()
                .Where(x => id.Equals(x.Id)
                            && x.Version <= maxVersion)
                .OrderByDescending(x => x.Version)
                .FirstOrDefaultAsync(cancellationToken);
        }

        protected virtual IQueryable<TAggregate> GetAggregateSet<TAggregate, TId>()
            where TAggregate : class, IAggregate<TId>
        {
            return _context.Set<TAggregate>().AsNoTracking();
        }
    }
}
