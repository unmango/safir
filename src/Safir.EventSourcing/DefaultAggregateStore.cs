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
        private readonly IEventSerializer _serializer;
        private readonly ILogger<DefaultAggregateStore> _logger;

        public DefaultAggregateStore(
            IEventStore store,
            IEventSerializer serializer,
            ILogger<DefaultAggregateStore> logger)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task StoreAsync<T>(T aggregate, CancellationToken cancellationToken = default)
            where T : IAggregate
        {
            _logger.LogTrace("Dequeuing and serializing events");
            var events = aggregate.DequeueEvents()
                .Select(e => _serializer.SerializeAsync(aggregate.Id, e, cancellationToken))
                .Select(x => x.AsTask())
                .ToList();
            
            if (events.Count <= 0) return;
            
            _logger.LogTrace("Adding events to event store");
            await _store.AddAsync(await Task.WhenAll(events), cancellationToken);
        }

        public ValueTask<T> GetAsync<T>(long id, CancellationToken cancellationToken = default)
            where T : IAggregate, new()
        {
            // TODO: This cancellation token situation...
            return _store.StreamAsync(id, cancellationToken)
                .DeserializeAsync(_serializer, cancellationToken)
                .AggregateAsync<T>(cancellationToken);
        }

        public ValueTask<T> GetAsync<T>(long id, int version, CancellationToken cancellationToken = default)
            where T : IAggregate, new()
        {
            // TODO: This cancellation token situation...
            return _store.StreamAsync(id, version, cancellationToken)
                .DeserializeAsync(_serializer, cancellationToken)
                .AggregateAsync<T>(cancellationToken);
        }
    }
}
