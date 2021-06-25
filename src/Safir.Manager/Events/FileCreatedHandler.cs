using System;
using System.Threading;
using System.Threading.Tasks;
using Faithlife.Utility;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Safir.Agent.Protos;
using Safir.EventSourcing;
using Safir.Messaging;

namespace Safir.Manager.Events
{
    [UsedImplicitly]
    internal sealed class FileCreatedHandler : IEventHandler<FileCreated>
    {
        private readonly IEventStore _eventStore;
        private readonly ILogger<FileCreatedHandler> _logger;

        public FileCreatedHandler(IEventStore eventStore, ILogger<FileCreatedHandler> logger)
        {
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public Task HandleAsync(FileCreated message, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Handling FileCreated event");
            _logger.LogTrace("Generating a guid from the file path");
            var id = GuidUtility.Create(GuidUtility.UrlNamespace, message.Path);
            
            _logger.LogTrace("Adding FileCreated event: {Id}", id);
            return _eventStore.AddAsync(id, message, cancellationToken);
        }
    }
}
