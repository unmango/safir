using System.Threading;
using System.Threading.Tasks;

namespace Safir.Common
{
    public interface IEventDispatcher
    {
        ValueTask DispatchAsync<T>(T notification, CancellationToken cancellationToken = default)
            where T : IEvent;
    }
}
