using System.Threading;
using System.Threading.Tasks;

namespace Safir.Messaging
{
    public interface IEventHandler { }
    
    public interface IEventHandler<in T> : IEventHandler
        where T : IEvent
    {
        Task HandleAsync(T message, CancellationToken cancellationToken = default);
    }
}
