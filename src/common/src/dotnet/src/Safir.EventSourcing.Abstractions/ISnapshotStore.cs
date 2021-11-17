using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public interface ISnapshotStore
    {
        Task AddAsync<TAggregate, TId>(TAggregate aggregate, CancellationToken cancellationToken = default)
            where TAggregate : IAggregate<TId>;

        Task<TAggregate> FindAsync<TAggregate, TId>(
            TId id,
            int maxVersion = int.MaxValue,
            CancellationToken cancellationToken = default)
            where TAggregate : class, IAggregate<TId>;
    }
}
