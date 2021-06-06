using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Safir.EventSourcing
{
    public class DefaultAggregateStore : IAggregateStore
    {
        private readonly IEventStore _store;
        private readonly IServiceProvider _services;
        private readonly ILogger<DefaultAggregateStore> _logger;

        public DefaultAggregateStore(IEventStore store, IServiceProvider services, ILogger<DefaultAggregateStore> logger)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StoreAsync<T>(T aggregate, CancellationToken cancellationToken = default)
            where T : IAggregate
        {
            _logger.LogTrace("Dequeuing events on aggregate {Id}", aggregate.Id);
            var events = aggregate.DequeueEvents().ToList();

            if (!events.Any()) return Task.CompletedTask;

            _logger.LogTrace("Adding events to event store");
            return _store.AddAsync(aggregate.Id, events, cancellationToken);
        }

        public Task StoreAsync<TAggregate, TId>(TAggregate aggregate, CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TId>
        {
            _logger.LogDebug("Fetching generic event store from service provider");
            var store = _services.GetRequiredService<IEventStore<TId>>();
            
            _logger.LogTrace("Dequeuing events on aggregate {Id}", aggregate.Id);
            var events = aggregate.DequeueEvents().ToList();

            if (!events.Any()) return Task.CompletedTask;

            _logger.LogTrace("Adding events to event store");
            return store.AddAsync(aggregate.Id, events, cancellationToken);
        }

        public ValueTask<T> GetAsync<T>(Guid id, CancellationToken cancellationToken = default)
            where T : IAggregate, new()
        {
            _logger.LogDebug("Getting event stream for aggregate {Id}", id);
            // TODO: This cancellation token situation...
            return _store.StreamAsync(id, cancellationToken)
                .AggregateAsync<T>(cancellationToken);
        }

        public ValueTask<TAggregate> GetAsync<TAggregate, TId>(TId id, CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TId>, new()
        {
            _logger.LogDebug("Fetching generic event store from service provider");
            var store = _services.GetRequiredService<IEventStore<TId>>();
            
            _logger.LogDebug("Getting event stream for aggregate {Id}", id);
            // TODO: This cancellation token situation...
            return store.StreamAsync(id, cancellationToken)
                .AggregateAsync<TAggregate, TId>(cancellationToken);
        }

        public ValueTask<T> GetAsync<T>(Guid id, int version, CancellationToken cancellationToken = default)
            where T : IAggregate, new()
        {
            _logger.LogDebug("Getting event stream for aggregate {Id} with version {Version}", id, version);
            // TODO: This cancellation token situation...
            return _store.StreamAsync(id, version, cancellationToken)
                .AggregateAsync<T>(cancellationToken);
        }

        public ValueTask<TAggregate> GetAsync<TAggregate, TId>(TId id, int version, CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TId>, new()
        {
            _logger.LogDebug("Fetching generic event store from service provider");
            var store = _services.GetRequiredService<IEventStore<TId>>();
            
            _logger.LogDebug("Getting event stream for aggregate {Id} with version {Version}", id, version);
            // TODO: This cancellation token situation...
            return store.StreamAsync(id, version, cancellationToken)
                .AggregateAsync<TAggregate, TId>(cancellationToken);
        }
    }
}
