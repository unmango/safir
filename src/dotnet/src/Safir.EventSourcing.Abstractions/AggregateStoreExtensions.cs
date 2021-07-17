using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public static class AggregateStoreExtensions
    {
        public static Task StoreAsync<T>(this IAggregateStore store, T aggregate, CancellationToken cancellationToken = default)
            where T : IAggregate
        {
            return store.StoreAsync<T, Guid>(aggregate, cancellationToken);
        }

        public static ValueTask<T> GetAsync<T>(this IAggregateStore store, Guid id, CancellationToken cancellationToken = default)
            where T : IAggregate, new()
        {
            return store.GetAsync<T, Guid>(id, cancellationToken);
        }

        public static ValueTask<T> GetAsync<T>(
            this IAggregateStore store,
            Guid id,
            int version,
            CancellationToken cancellationToken = default)
            where T : IAggregate, new()
        {
            return store.GetAsync<T, Guid>(id, version, cancellationToken);
        }
    }
}
