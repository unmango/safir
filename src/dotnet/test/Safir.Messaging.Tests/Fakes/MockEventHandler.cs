using System.Threading;
using System.Threading.Tasks;

namespace Safir.Messaging.Tests.Fakes
{
    public class MockEventHandler : IEventHandler<MockEvent>
    {
        public Task HandleAsync(MockEvent message, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
