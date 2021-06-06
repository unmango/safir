using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public interface IAggregateStore
    {
        Task StoreAsync<T>(T aggregate, CancellationToken cancellationToken = default)
            where T : IAggregate;

        Task StoreAsync<TAggregate, TId>(TAggregate aggregate, CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TId>;

        ValueTask<T> GetAsync<T>(Guid id, CancellationToken cancellationToken = default)
            where T : IAggregate, new();

        ValueTask<TAggregate> GetAsync<TAggregate, TId>(TId id, CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TId>, new();

        ValueTask<T> GetAsync<T>(Guid id, int version, CancellationToken cancellationToken = default)
            where T : IAggregate, new();

        ValueTask<TAggregate> GetAsync<TAggregate, TId>(TId id, int version, CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TId>, new();
    }
}
