using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Safir.EventSourcing
{
    public class DefaultAggregateStore<T> : DefaultAggregateStore<T, Guid>, IAggregateStore<T>
        where T : IAggregate, new()
    {
        public DefaultAggregateStore(IEventStore eventStore, ILogger<DefaultAggregateStore<T>> logger)
            : base(eventStore, logger) { }
    }

    public class DefaultAggregateStore<TAggregate, TId> : DefaultAggregateStore<TAggregate, TId, Guid>
        where TAggregate : IAggregate<TId>, new()
    {
        public DefaultAggregateStore(
            IEventStore<TId, Guid> eventStore,
            ILogger<DefaultAggregateStore<TAggregate, TId>> logger)
            : base(eventStore, logger) { }
    }

    public class DefaultAggregateStore<TAggregate, TAggregateId, TEventId> : IAggregateStore<TAggregate, TAggregateId>
        where TAggregate : IAggregate<TAggregateId>, new()
    {
        private readonly IEventStore<TAggregateId, TEventId> _eventStore;
        private readonly ILogger<DefaultAggregateStore<TAggregate, TAggregateId, TEventId>> _logger;

        public DefaultAggregateStore(
            IEventStore<TAggregateId, TEventId> eventStore,
            ILogger<DefaultAggregateStore<TAggregate, TAggregateId, TEventId>> logger)
        {
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StoreAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Dequeuing events on aggregate {Id}", aggregate.Id);
            var events = aggregate.DequeueEvents().ToList();

            if (!events.Any()) return Task.CompletedTask;

            _logger.LogTrace("Adding events to event store");
            return _eventStore.AddAsync(aggregate.Id, events, cancellationToken);
        }

        public ValueTask<TAggregate> GetAsync(TAggregateId id, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Getting event stream for aggregate {Id}", id);
            // TODO: This cancellation token situation...
            return _eventStore.StreamAsync(id, cancellationToken)
                .AggregateAsync<TAggregate, TAggregateId>(cancellationToken);
        }

        public ValueTask<TAggregate> GetAsync(TAggregateId id, int version, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Getting event stream for aggregate {Id} with version {Version}", id, version);
            // TODO: This cancellation token situation...
            return _eventStore.StreamAsync(id, version, cancellationToken)
                .AggregateAsync<TAggregate, TAggregateId>(cancellationToken);
        }
    }
}
