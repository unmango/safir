using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Safir.Common.Tasks;
using Safir.Messaging;

namespace Safir.EventSourcing.EntityFrameworkCore
{
    [PublicAPI]
    public class DbContextEventStore<TContext> : IEventStore
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly IEventSerializer _serializer;
        private readonly ILogger<DbContextEventStore<TContext>> _logger;

        public DbContextEventStore(TContext context, IEventSerializer serializer, ILogger<DbContextEventStore<TContext>> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task AddAsync<TAggregateId>(
            TAggregateId aggregateId,
            IEvent @event,
            CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Serializing event {Event}", @event);
            var serialized = await _serializer.SerializeAsync(aggregateId, @event, cancellationToken);
            _logger.LogTrace("Serialized event {Event}", @event);

            _logger.LogTrace("Adding event {Event}", @event);
            await _context.AddAsync(serialized, cancellationToken);
            _logger.LogTrace("Added event {Event}", @event);

            _logger.LogTrace("Saving changes asynchronously");
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogTrace("Saved changes asynchronously");
        }

        public async Task AddAsync<TAggregateId>(
            TAggregateId aggregateId,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Serializing events {Events}", events);
            var serialized = await events.Select(x => _serializer.SerializeAsync(aggregateId, x, cancellationToken));
            _logger.LogTrace("Serialized events {Events}", events);

            _logger.LogTrace("Adding events {Events}", events);
            await _context.AddRangeAsync(serialized, cancellationToken);
            _logger.LogTrace("Added events {Events}", events);

            _logger.LogTrace("Saving changes asynchronously");
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogTrace("Saved changes asynchronously");
        }

        public Task<IEvent> GetAsync<TAggregateId>(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            _logger.LogTrace("Getting single event with id {Id}", id);
            return GetEventSet<TAggregateId>().SingleAsync(x => id.Equals(x.Id), cancellationToken)
                .Bind(x => Deserialize(x, cancellationToken));
        }

        public Task<TEvent> GetAsync<TEvent, TAggregateId>(Guid id, CancellationToken cancellationToken = default)
            where TEvent : IEvent
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            _logger.LogTrace("Getting single event with id {Id}", id);
            return GetEventSet<TAggregateId>().SingleAsync(x => id.Equals(x.Id), cancellationToken)
                .Bind(x => Deserialize<TEvent, TAggregateId>(x, cancellationToken));
        }

        public IAsyncEnumerable<IEvent> StreamAsync<TAggregateId>(
            TAggregateId aggregateId,
            int startPosition = 0,
            int endPosition = int.MaxValue,
            CancellationToken cancellationToken = default)
        {
            if (aggregateId == null) throw new ArgumentNullException(nameof(aggregateId));

            if (startPosition > endPosition)
            {
                throw new InvalidOperationException("Start position can't be after the end position");
            }

            _logger.LogTrace(
                "Streaming events with aggregateId {AggregateId} from start {Start} till end {End}",
                aggregateId,
                startPosition,
                endPosition);

            return GetEventSet<TAggregateId>()
                .Where(x => aggregateId.Equals(x.AggregateId)
                            && x.Position >= startPosition
                            && x.Position <= endPosition)
                .AsAsyncEnumerable()
                .DeserializeAsync(_serializer, cancellationToken);
        }

        public IAsyncEnumerable<IEvent> StreamBackwardsAsync<TAggregateId>(
            TAggregateId aggregateId,
            int? count = null,
            CancellationToken cancellationToken = default)
        {
            if (aggregateId == null) throw new ArgumentNullException(nameof(aggregateId));

            var logCount = count.HasValue ? $"{count}" : "all";
            _logger.LogTrace("Streaming {Count} event(s) backwards with aggregate Id {Id}", logCount, aggregateId);
            return GetEventSet<TAggregateId>()
                .Where(x => aggregateId.Equals(x.AggregateId))
                .OrderByDescending(x => x.Position)
                .Take(count ?? int.MaxValue)
                .AsAsyncEnumerable()
                .DeserializeAsync(_serializer, cancellationToken);
        }

        protected virtual IQueryable<Event<TAggregateId>> GetEventSet<TAggregateId>()
        {
            return _context.Set<Event<TAggregateId>>().AsNoTracking();
        }

        private Task<IEvent> Deserialize<TAggregateId>(Event<TAggregateId> @event, CancellationToken cancellationToken)
        {
            return _serializer.DeserializeAsync(@event, cancellationToken).AsTask();
        }

        // TODO: Fix order of generic params
        private Task<TEvent> Deserialize<TEvent, TAggregateId>(
            Event<TAggregateId> @event,
            CancellationToken cancellationToken)
            where TEvent : IEvent
        {
            return _serializer.DeserializeAsync<TAggregateId, TEvent>(@event, cancellationToken).AsTask();
        }
    }
}
