using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.Messaging.MediatR
{
    [UsedImplicitly]
    public abstract class SyncNotificationHandler<T> : NotificationHandler<T>
        where T : IEvent
    {
        protected override Task Handle(T notification, CancellationToken cancellationToken)
        {
            Handle(notification);
            
            return Task.CompletedTask;
        }

        protected abstract void Handle(T notification);
    }
}
