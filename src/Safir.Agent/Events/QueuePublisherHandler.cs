using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Safir.Agent.Events
{
    // TODO: I don't think MS DI supports polymorphic dispatch
    [UsedImplicitly]
    internal sealed class QueuePublisherHandler : INotificationHandler<INotification>
    {
        private readonly ILogger<QueuePublisherHandler> _logger;

        public QueuePublisherHandler(ILogger<QueuePublisherHandler> logger)
        {
            _logger = logger;
        }
        
        public Task Handle(INotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("TODO: publish to queue");
            
            return Task.CompletedTask;
        }
    }
}
