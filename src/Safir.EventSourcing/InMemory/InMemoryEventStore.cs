using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Safir.Messaging;

namespace Safir.EventSourcing.InMemory
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly ILogger<InMemoryEventStore> _logger;
        private readonly ConcurrentDictionary<Guid, IEvent> _events = new();
        private readonly ConcurrentDictionary<Guid, object> _aggregateMap = new();

        public InMemoryEventStore(ILogger<InMemoryEventStore> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task AddAsync<TAggregateId>(TAggregateId aggregateId, IEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Checking for null aggregateId");
            if (aggregateId == null) throw new ArgumentNullException(nameof(aggregateId));
            
            _logger.LogTrace("Generating new Guid event id");
            var id = Guid.NewGuid();
            
            _logger.LogTrace("Adding event with id {Id}", id);
            var eventAdded = _events.TryAdd(id, @event);
            _logger.LogTrace("Added event with id {Id}: {Added}", id, eventAdded);
            
            _logger.LogTrace("Mapping event {Id} to aggregate {AggregateId}", id, aggregateId);
            var aggregateMapped = _aggregateMap.TryAdd(id, aggregateId);
            _logger.LogTrace("Event {Id} mapped to {AggregateId}: {Mapped}", id, aggregateId, aggregateMapped);
            
            return Task.CompletedTask;
        }

        public Task AddAsync<TAggregateId>(
            TAggregateId aggregateId,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Adding all events to aggregate {Id}", aggregateId);
            var tasks = events.Select(x => AddAsync(aggregateId, x, cancellationToken));
            
            _logger.LogTrace("Waiting for all events to add");
            return Task.WhenAll(tasks);
        }

        public Task<IEvent> GetAsync<TAggregateId>(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                return Task.FromResult(_events[id]);
            }
            catch (KeyNotFoundException e)
            {
                return Task.FromException<IEvent>(e);
            }
        }

        public Task<TEvent> GetAsync<TEvent, TAggregateId>(Guid id, CancellationToken cancellationToken = default)
            where TEvent : IEvent
        {
            try
            {
                return Task.FromResult((TEvent)_events[id]);
            }
            catch (Exception e) when (e is KeyNotFoundException or InvalidCastException)
            {
                return Task.FromException<TEvent>(e);
            }
        }

        // TODO: WTF make sync? Bleh
        public async IAsyncEnumerable<IEvent> StreamAsync<TAggregateId>(
            TAggregateId aggregateId,
            int startPosition = 0,
            int endPosition = int.MaxValue,
            CancellationToken cancellationToken = default)
        {
            if (aggregateId == null) throw new ArgumentNullException(nameof(aggregateId));
            
            var ids = _aggregateMap.Where(x => aggregateId.Equals(x.Value));
            var events = _events.Join(ids, x => x.Key, x => x.Key, (x, _) => x.Value);

            foreach (var @event in events)
                yield return @event;
        }

        public IAsyncEnumerable<IEvent> StreamBackwardsAsync<TAggregateId>(
            TAggregateId aggregateId,
            int? count = null,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
