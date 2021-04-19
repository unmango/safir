using System;
using MediatR;

namespace Safir.Messaging.MediatR
{
    internal record NotificationWrapper<T>(T Value) : INotification where T : IEvent;
    
    public static class EventExtensions
    {
        public static INotification AsNotification<T>(this T @event)
            where T : IEvent
        {
            return new NotificationWrapper<T>(@event);
        }

        public static T GetEvent<T>(this INotification notification)
            where T : IEvent
        {
            if (notification is not NotificationWrapper<T> wrapper)
            {
                throw new NotSupportedException("Unable to unwrap notification");
            }

            return wrapper.Value;
        }
    }
}
