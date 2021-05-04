using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Safir.Messaging.Tests.Fakes;
using Xunit;

namespace Safir.Messaging.Tests
{
    public class DefaultTypedEventBusTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IEventBus> _eventBus;
        private readonly DefaultTypedEventBus<MockEvent> _typedEventBus;

        public DefaultTypedEventBusTests()
        {
            _eventBus = _mocker.GetMock<IEventBus>();
            _typedEventBus = _mocker.CreateInstance<DefaultTypedEventBus<MockEvent>>();
        }

        [Fact]
        public void Subscribe_DelegatesSubscribeToGenericBus()
        {
            var observer = _mocker.Get<IObserver<MockEvent>>();
            var observable = _mocker.GetMock<IObservable<MockEvent>>();
            _eventBus.Setup(x => x.GetObservable<MockEvent>()).Returns(observable.Object);

            _typedEventBus.Subscribe(observer);
            
            _eventBus.Verify(x => x.GetObservable<MockEvent>());
            observable.Verify(x => x.Subscribe(observer));
        }

        [Fact]
        public async Task PublishAsync_DelegatesPublishToGenericBus()
        {
            var message = new MockEvent();

            await _typedEventBus.PublishAsync(message);
            
            _eventBus.Verify(x => x.PublishAsync(message, It.IsAny<CancellationToken>()));
        }
    }
}
