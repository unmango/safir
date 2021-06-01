using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Safir.EventSourcing.EntityFrameworkCore
{
    [PublicAPI]
    public class DbContextEventStore<TContext> : IEventStore
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly ILogger<DbContextEventStore<TContext>> _logger;

        public DbContextEventStore(TContext context, ILogger<DbContextEventStore<TContext>> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task AddAsync(Event @event, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Adding event {Event}", @event);
            await _context.AddAsync(@event, cancellationToken);
            _logger.LogTrace("Added event {Event}", @event);

            _logger.LogTrace("Saving changes asynchronously");
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogTrace("Saved changes asynchronously");
        }

        public async Task AddAsync(IEnumerable<Event> events, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Adding events {Events}", events);
            await _context.AddRangeAsync(events, cancellationToken);
            _logger.LogTrace("Added events {Events}", events);

            _logger.LogTrace("Saving changes asynchronously");
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogTrace("Saved changes asynchronously");
        }

        public Task<Event> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Getting single event with id {Id}", id);
            return GetEventSet().AsQueryable().FirstAsync(x => x.Id == id, cancellationToken);
        }

        public IAsyncEnumerable<Event> StreamBackwardsAsync(
            long aggregateId,
            int? count = null,
            CancellationToken cancellationToken = default)
        {
            var logCount = count.HasValue ? $"{count}" : "all";
            _logger.LogTrace("Streaming {Count} events backwards with aggregate Id {Id}", logCount, aggregateId);
            return GetEventSet().OrderByDescending(x => x.Position).Take(count ?? int.MaxValue).AsAsyncEnumerable();
        }

        public IAsyncEnumerable<Event> StreamAsync(
            long aggregateId,
            int startPosition = int.MinValue,
            int endPosition = int.MaxValue,
            CancellationToken cancellationToken = default)
        {
            if (startPosition > endPosition)
            {
                throw new InvalidOperationException("Start position can't be after the end position");
            }
            
            _logger.LogTrace(
                "Streaming events with aggregateId {AggregateId} from start {Start} till end {End}",
                aggregateId,
                startPosition,
                endPosition);

            return GetEventSet()
                .Where(x => x.AggregateId == aggregateId
                            && x.Position >= startPosition
                            && x.Position <= endPosition)
                .AsAsyncEnumerable();
        }

        protected virtual IQueryable<Event> GetEventSet()
        {
            return _context.Set<Event>().AsNoTracking();
        }
    }
}
