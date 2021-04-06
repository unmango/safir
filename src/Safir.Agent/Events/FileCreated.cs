using MediatR;

namespace Safir.Agent.Events
{
    internal record FileCreated(string Path) : INotification;
}
