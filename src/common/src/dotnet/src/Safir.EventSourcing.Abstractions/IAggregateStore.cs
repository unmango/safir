using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public interface IAggregateStore
    {
        Task StoreAsync<TAggregate, TId>(TAggregate aggregate, CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TId>;

        ValueTask<TAggregate> GetAsync<TAggregate, TId>(TId id, CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TId>, new();

        ValueTask<TAggregate> GetAsync<TAggregate, TId>(TId id, int version, CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TId>, new();
    }
}
