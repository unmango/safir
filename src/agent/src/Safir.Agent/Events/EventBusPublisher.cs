using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Safir.Agent.Protos;
using Safir.Messaging;
using Safir.Messaging.MediatR;

namespace Safir.Agent.Events
{
    [UsedImplicitly]
    internal sealed class EventBusPublisher :
        INotificationHandler<Notification<FileCreated>>,
        INotificationHandler<Notification<FileChanged>>,
        INotificationHandler<Notification<FileDeleted>>,
        INotificationHandler<Notification<FileRenamed>>
    {
        private readonly IEventBus _bus;
        private readonly ILogger<EventBusPublisher> _logger;

        public EventBusPublisher(IEventBus bus, ILogger<EventBusPublisher> logger)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _logger = logger;
        }

        public Task Handle(Notification<FileCreated> notification, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Publishing created event to bus");
            return TryPublishAsync(notification, cancellationToken);
        }

        public Task Handle(Notification<FileChanged> notification, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Publishing changed event to bus");
            return TryPublishAsync(notification, cancellationToken);
        }

        public Task Handle(Notification<FileDeleted> notification, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Publishing deleted event to bus");
            return TryPublishAsync(notification, cancellationToken);
        }

        public Task Handle(Notification<FileRenamed> notification, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Publishing renamed event to bus");
            return TryPublishAsync(notification, cancellationToken);
        }

        private async Task TryPublishAsync<T>(Notification<T> notification, CancellationToken cancellationToken)
            where T : IEvent
        {
            try
            {
                await _bus.PublishAsync(notification, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to publish event to bus");
            }
        }
    }
}
