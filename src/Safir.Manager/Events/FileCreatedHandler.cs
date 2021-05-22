using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Safir.Agent.Protos;
using Safir.Manager.Data;
using Safir.Manager.Domain;
using Safir.Messaging;

namespace Safir.Manager.Events
{
    [UsedImplicitly]
    internal sealed class FileCreatedHandler : IEventHandler<FileCreated>
    {
        private readonly ManagerContext _context;
        private readonly ILogger<FileCreatedHandler> _logger;

        public FileCreatedHandler(ManagerContext context, ILogger<FileCreatedHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }
        
        public async Task HandleAsync(FileCreated message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handling file created event");
            
            
            
            var file = new File(message.Path, "TODO", 12345);
            _context.Files.
            
            throw new NotImplementedException();
        }
    }
}
