using MediatR;

namespace Safir.Messaging.MediatR
{
    public record Notification<T>(T Value) : INotification
        where T : IEvent;
}
