using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public interface IAggregateStore<T>
        where T : IAggregate
    {
        Task StoreAsync(T aggregate, CancellationToken cancellationToken = default);

        ValueTask<T> GetAsync(long id, CancellationToken cancellationToken = default);

        ValueTask<T> GetAsync(long id, int version, CancellationToken cancellationToken = default);
    }
}
