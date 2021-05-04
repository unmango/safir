using System;
using JetBrains.Annotations;
using MediatR;

namespace Safir.Messaging.MediatR
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class EventExtensions
    {
        public static INotification AsNotification<T>(this T @event)
            where T : IEvent
        {
            return new Notification<T>(@event);
        }

        public static T GetEvent<T>(this INotification notification)
            where T : IEvent
        {
            if (notification is not Notification<T> wrapper)
            {
                throw new NotSupportedException("Unable to unwrap notification");
            }

            return wrapper.Value;
        }
    }
}
