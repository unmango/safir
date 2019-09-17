using System.Threading;
using System.Threading.Tasks;

namespace Safir.Common
{
    public static class EventHelper
    {
        public static ValueTask DispatchAsync(
            IDispatchEvents provider,
            IEventDispatcher dispatcher,
            CancellationToken cancellationToken = default)
        {
            return provider.DispatchEventsAsync(dispatcher, cancellationToken);
        }
    }
}
