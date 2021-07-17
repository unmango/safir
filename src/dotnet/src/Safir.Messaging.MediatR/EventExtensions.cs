using System;
using JetBrains.Annotations;
using MediatR;
using Safir.Common;

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

        public static IEvent GetEvent(this INotification notification)
        {
            // ReSharper disable once UseIsOperator.2
            if (!notification.GetType().IsAssignableToGeneric(typeof(Notification<>)))
            {
                throw new NotSupportedException("Unable to unwrap notification");
            }

            var eventType = notification.GetType().GetGenericArguments()[0];
            var closed = typeof(Notification<>).MakeGenericType(eventType);
            return (IEvent)closed.GetProperty("Value")!.GetValue(notification);
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
