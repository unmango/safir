using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Common
{
    public static class DispatchEventsExtensions
    {
        public static ValueTask DispatchEventsAsync(
            this IDispatchEvents provider,
            IEventDispatcher dispatcher,
            CancellationToken cancellationToken = default)
        {
            var entities = provider.GetEntities().ToList();

            if (entities.Count <= 0) return new ValueTask();

            var tasks = entities
                .SelectMany(x => x.Events)
                .Select(e => dispatch(e).AsTask());

            entities.ForEach(x => x.ClearEvents());

            return new ValueTask(Task.WhenAll(tasks));

            ValueTask dispatch(IEvent e) => dispatcher.DispatchAsync(e, cancellationToken);
        }
    }
}
