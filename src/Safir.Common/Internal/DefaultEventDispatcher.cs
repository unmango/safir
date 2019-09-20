using System.Threading;
using System.Threading.Tasks;

namespace Safir.Common.Internal
{
    internal class DefaultEventDispatcher : IEventDispatcher
    {
        public ValueTask DispatchAsync<T>(T notification, CancellationToken cancellationToken = default) where T : IEvent
        {
            return new ValueTask();
        }
    }
}
