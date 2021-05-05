using Safir.Messaging.MediatR.Tests.Fakes;
using Xunit;

namespace Safir.Messaging.MediatR.Tests
{
    public class EventExtensionsTests
    {
        [Fact]
        public void GetEvent_GetsEventFromNonGenericNotification()
        {
            var @event = new FakeEvent();
            var notification = new Notification<FakeEvent>(@event);

            var result = notification.GetEvent();
            
            Assert.NotNull(result);
            Assert.Same(@event, result);
        }
    }
}
