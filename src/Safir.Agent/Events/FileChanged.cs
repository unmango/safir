using MediatR;

namespace Safir.Agent.Events
{
    public record FileChanged(string Path) : INotification;
}
