using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.Messaging
{
    [UsedImplicitly]
    public abstract class SynchronousEventHandler<T> : IEventHandler<T>
        where T : IEvent
    {
        public Task HandleAsync(T message, CancellationToken cancellationToken = default)
        {
            Handle(message);
            
            return Task.CompletedTask;
        }

        protected abstract void Handle(T message);
    }
}
