using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Agent.Protos;
using Safir.Manager.Data;
using Safir.Messaging;

namespace Safir.Manager.Events
{
    [UsedImplicitly]
    internal sealed class FileCreatedHandler : IEventHandler<FileCreated>
    {
        private readonly ManagerContext _context;

        public FileCreatedHandler(ManagerContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async Task HandleAsync(FileCreated message, CancellationToken cancellationToken = default)
        {
            var entity = new Domain.FileCreated(message.Path, "TODO");
            var added = await _context.FileCreated.AddAsync(entity, cancellationToken);
        }
    }
}
