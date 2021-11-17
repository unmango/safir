using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Safir.Messaging.MediatR.Tests.Fakes;
using Xunit;

namespace Safir.Messaging.MediatR.Tests
{
    public class EventBusNotificationPublisherTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IEventBus> _bus;
        private readonly EventBusNotificationPublisher<FakeEvent> _publisher;
        private readonly CancellationToken _cancellationToken = default;

        public EventBusNotificationPublisherTests()
        {
            _bus = _mocker.GetMock<IEventBus>();
            _publisher = _mocker.CreateInstance<EventBusNotificationPublisher<FakeEvent>>();
        }

        [Fact]
        public async Task Handle_PublishesNotificationValueToEventBus()
        {
            var @event = new FakeEvent();
            var notification = new Notification<FakeEvent>(@event);

            await _publisher.Handle(notification, _cancellationToken);
            
            _bus.Verify(x => x.PublishAsync(@event, _cancellationToken));
        }
    }
}
