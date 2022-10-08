using Safir.Agent.Protos;
using Safir.Messaging;

namespace Safir.Manager.Events;

public class FileCreatedHandler : IEventHandler<FileCreated>
{
    public Task HandleAsync(FileCreated message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
