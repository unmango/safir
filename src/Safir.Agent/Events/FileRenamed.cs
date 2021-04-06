using MediatR;

namespace Safir.Agent.Events
{
    public record FileRenamed(string Path) : INotification;
}
