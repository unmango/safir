using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Safir.Messaging.MediatR
{
    internal sealed class EventBusNotificationPublisher<T> : INotificationHandler<Notification<T>>
        where T : IEvent
    {
        private readonly IEventBus _bus;
        private readonly ILogger<EventBusNotificationPublisher<T>> _logger;

        public EventBusNotificationPublisher(IEventBus bus, ILogger<EventBusNotificationPublisher<T>> logger)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _logger = logger;
        }
        
        public Task Handle(Notification<T> notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Publishing notification as event to bus: {Type}", notification.Value.GetType().Name);
            return _bus.PublishAsync(notification.Value, cancellationToken);
        }
    }
}
