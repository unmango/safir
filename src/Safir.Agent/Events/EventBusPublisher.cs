using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Safir.Agent.Events
{
    [UsedImplicitly]
    internal sealed class EventBusPublisher :
        INotificationHandler<FileCreated>,
        INotificationHandler<FileChanged>,
        INotificationHandler<FileDeleted>,
        INotificationHandler<FileRenamed>
    {
        private readonly ILogger<EventBusPublisher> _logger;

        public EventBusPublisher(ILogger<EventBusPublisher> logger)
        {
            _logger = logger;
        }

        public Task Handle(FileCreated notification, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Publishing created event to bus");
            return Task.CompletedTask;
        }

        public Task Handle(FileChanged notification, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Publishing changed event to bus");
            return Task.CompletedTask;
        }

        public Task Handle(FileDeleted notification, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Publishing deleted event to bus");
            return Task.CompletedTask;
        }

        public Task Handle(FileRenamed notification, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Publishing renamed event to bus");
            return Task.CompletedTask;
        }
    }
}
