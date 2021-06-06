using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public interface IAggregateStore<T> : IAggregateStore<T, Guid>
        where T : IAggregate { }
    
    [PublicAPI]
    public interface IAggregateStore<TAggregate, in TId>
        where TAggregate : IAggregate<TId>
    {
        Task StoreAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

        ValueTask<TAggregate> GetAsync(TId id, CancellationToken cancellationToken = default);

        ValueTask<TAggregate> GetAsync(TId id, int version, CancellationToken cancellationToken = default);
    }
}
