using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;

namespace Safir.Messaging.MediatR
{
    public abstract class NotificationHandler<T> : INotificationHandler<Notification<T>>
        where T : IEvent
    {
        Task INotificationHandler<Notification<T>>.Handle(
            Notification<T> notification,
            CancellationToken cancellationToken)
        {
            return Handle(notification.Value, cancellationToken);
        }

        protected abstract Task Handle(T notification, [UsedImplicitly] CancellationToken cancellationToken);
    }
}
