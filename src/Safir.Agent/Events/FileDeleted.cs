using MediatR;

namespace Safir.Agent.Events
{
    public record FileDeleted(string Path) : INotification;
}
