using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public static class SnapshotStoreExtensions
    {
        public static Task<TAggregate> FindAsync<TAggregate, TId>(
            this ISnapshotStore store,
            TId id,
            CancellationToken cancellationToken)
            where TAggregate : class, IAggregate<TId>
        {
            return store.FindAsync<TAggregate, TId>(id, cancellationToken: cancellationToken);
        }
        
        public static Task<T> FindAsync<T>(
            this ISnapshotStore store,
            Guid id,
            int maxVersion = int.MaxValue,
            CancellationToken cancellationToken = default)
            where T : class, IAggregate
        {
            return store.FindAsync<T, Guid>(id, maxVersion, cancellationToken);
        }
        
        public static Task<T> FindAsync<T>(
            this ISnapshotStore store,
            Guid id,
            CancellationToken cancellationToken)
            where T : class, IAggregate
        {
            return store.FindAsync<T, Guid>(id, cancellationToken);
        }
    }
}
