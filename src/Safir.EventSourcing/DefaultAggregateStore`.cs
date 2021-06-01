using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Safir.EventSourcing
{
    public class DefaultAggregateStore<T> : IAggregateStore<T>
        where T : IAggregate, new()
    {
        private readonly IAggregateStore _aggregateStore;
        private readonly ILogger<DefaultAggregateStore<T>> _logger;

        public DefaultAggregateStore(IAggregateStore aggregateStore, ILogger<DefaultAggregateStore<T>> logger)
        {
            _aggregateStore = aggregateStore ?? throw new ArgumentNullException(nameof(aggregateStore));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StoreAsync(T aggregate, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Delegating store operation to generic aggregate store");
            return _aggregateStore.StoreAsync(aggregate, cancellationToken);
        }

        public ValueTask<T> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Delegating get operation to generic aggregate store");
            return _aggregateStore.GetAsync<T>(id, cancellationToken);
        }

        public ValueTask<T> GetAsync(long id, int version, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Delegating get operation to generic aggregate store");
            return _aggregateStore.GetAsync<T>(id, version, cancellationToken);
        }
    }
}
