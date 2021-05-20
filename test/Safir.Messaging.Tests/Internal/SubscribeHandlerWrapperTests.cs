using System;
using System.Threading;
using Moq;
using Moq.AutoMock;
using Safir.Messaging.Internal;
using Safir.Messaging.Tests.Fakes;
using Xunit;

namespace Safir.Messaging.Tests.Internal
{
    public class SubscribeHandlerWrapperTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IEventBus> _eventBus;
        private readonly Mock<IEventHandler> _genericHandler;
        private readonly Mock<IEventHandler<MockEvent>> _typedHandler;
        private readonly SubscribeHandlerWrapper<MockEvent> _wrapper = new();

        public SubscribeHandlerWrapperTests()
        {
            _eventBus = _mocker.GetMock<IEventBus>();
            _genericHandler = _mocker.GetMock<IEventHandler>();
            _typedHandler = _mocker.GetMock<IEventHandler<MockEvent>>();
        }

        [Fact]
        public void Subscribe_ThrowsWhenGenericHandler()
        {
            Assert.Throws<InvalidOperationException>(
                () => _wrapper.Subscribe(_eventBus.Object, _genericHandler.Object));
        }

        [Fact]
        public void Subscribe_ThrowsWhenInvalidHandlerType()
        {
            var handler = _mocker.GetMock<IEventHandler<DifferentEvent>>();
            
            Assert.Throws<InvalidOperationException>(
                () => _wrapper.Subscribe(_eventBus.Object, handler.Object));
        }

        [Fact]
        public void Subscribe_SubscribesHandler()
        {
            IObserver<MockEvent>? observer = null;
            _eventBus.Setup(x => x.SubscribeAsync(It.IsAny<IObserver<MockEvent>>(), It.IsAny<CancellationToken>()))
                .Callback<IObserver<MockEvent>, CancellationToken>((x, _) => observer = x);
            var message = new MockEvent();
            
            var subscription = _wrapper.Subscribe(_eventBus.Object, _typedHandler.Object);
            
            Assert.NotNull(subscription);
            _eventBus.Verify(x => x.SubscribeAsync(It.IsAny<IObserver<MockEvent>>(), It.IsAny<CancellationToken>()));
            Assert.NotNull(observer);

            observer?.OnNext(message);
            
            _typedHandler.Verify(x => x.HandleAsync(It.IsAny<MockEvent>(), It.IsAny<CancellationToken>()));
        }

        public record DifferentEvent : IEvent;
    }
}
