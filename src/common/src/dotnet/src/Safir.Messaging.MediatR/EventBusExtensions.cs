using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.Messaging.MediatR
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class EventBusExtensions
    {
        public static Task PublishAsync<T>(
            this IEventBus bus,
            Notification<T> notification,
            CancellationToken cancellationToken = default)
            where T : IEvent
        {
            return bus.PublishAsync(notification.Value, cancellationToken);
        }
    }
}
