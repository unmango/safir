using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Safir.EventSourcing
{
    public class DefaultAggregateStore : IAggregateStore
    {
        private readonly IEventStore _store;
        private readonly ILogger<DefaultAggregateStore> _logger;

        public DefaultAggregateStore(IEventStore store, ILogger<DefaultAggregateStore> logger)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StoreAsync<TAggregate, TId>(TAggregate aggregate, CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TId>
        {
            _logger.LogTrace("Dequeuing events on aggregate {Id}", aggregate.Id);
            var events = aggregate.DequeueEvents().ToList();

            if (!events.Any()) return Task.CompletedTask;

            _logger.LogTrace("Adding events to event store");
            return _store.AddAsync(aggregate.Id, events, cancellationToken);
        }

        public ValueTask<TAggregate> GetAsync<TAggregate, TId>(TId id, CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TId>, new()
        {
            _logger.LogDebug("Getting event stream for aggregate {Id}", id);
            // TODO: This cancellation token situation...
            return _store.StreamAsync(id, cancellationToken)
                .AggregateAsync<TAggregate, TId>(cancellationToken);
        }

        public ValueTask<TAggregate> GetAsync<TAggregate, TId>(TId id, int version, CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TId>, new()
        {
            _logger.LogDebug("Getting event stream for aggregate {Id} with version {Version}", id, version);
            // TODO: This cancellation token situation...
            return _store.StreamAsync(id, version, cancellationToken)
                .AggregateAsync<TAggregate, TId>(cancellationToken);
        }
    }
}
