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

        public async Task AddAsync(Guid aggregateId, IEvent @event, CancellationToken cancellationToken = default)
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

        public async Task AddAsync(Guid aggregateId, IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
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

        public Task<IEvent> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Getting single event with id {Id}", id);
            return GetEventSet().SingleAsync(x => x.Id == id, cancellationToken)
                .Bind(x => Deserialize(x, cancellationToken));
        }

        public Task<T> GetAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : IEvent
        {
            _logger.LogTrace("Getting single event with id {Id}", id);
            return GetEventSet().SingleAsync(x => x.Id == id, cancellationToken)
                .Bind(x => Deserialize<T>(x, cancellationToken));
        }

        public IAsyncEnumerable<IEvent> StreamBackwardsAsync(
            Guid aggregateId,
            int? count = null,
            CancellationToken cancellationToken = default)
        {
            var logCount = count.HasValue ? $"{count}" : "all";
            _logger.LogTrace("Streaming {Count} event(s) backwards with aggregate Id {Id}", logCount, aggregateId);
            return GetEventSet()
                .Where(x => x.AggregateId == aggregateId)
                .OrderByDescending(x => x.Position)
                .Take(count ?? int.MaxValue)
                .AsAsyncEnumerable()
                .DeserializeAsync(_serializer, cancellationToken);
        }

        public IAsyncEnumerable<IEvent> StreamAsync(
            Guid aggregateId,
            int startPosition = 0,
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
                .AsAsyncEnumerable()
                .DeserializeAsync(_serializer, cancellationToken);
        }

        protected virtual IQueryable<Event> GetEventSet()
        {
            return _context.Set<Event>().AsNoTracking();
        }

        private Task<IEvent> Deserialize(Event @event, CancellationToken cancellationToken)
        {
            return _serializer.DeserializeAsync(@event, cancellationToken).AsTask();
        }

        private Task<T> Deserialize<T>(Event @event, CancellationToken cancellationToken)
            where T : IEvent
        {
            return _serializer.DeserializeAsync<T>(@event, cancellationToken).AsTask();
        }
    }
}
