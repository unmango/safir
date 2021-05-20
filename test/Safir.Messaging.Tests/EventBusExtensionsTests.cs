using System;
using System.Reactive.Linq;
using System.Threading;
using Moq;
using Moq.AutoMock;
using Safir.Messaging.Tests.Fakes;
using Xunit;

// ReSharper disable ConvertToLocalFunction

namespace Safir.Messaging.Tests
{
    public class EventBusExtensionsTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IEventBus> _eventBus;
        private readonly CancellationToken _cancellationToken = default;

        public EventBusExtensionsTests()
        {
            _eventBus = _mocker.GetMock<IEventBus>();
        }

        [Fact]
        public void GetObservable_SubscribesToEvent()
        {
            var observable = _eventBus.Object.GetObservable<MockEvent>();

            Assert.NotNull(observable);

            observable.Subscribe();
            
            _eventBus.Verify(x => x.SubscribeAsync(It.IsAny<IObserver<MockEvent>>(), _cancellationToken));
        }

        [Fact]
        public void GetObservable_ThrowsWhenSubscribed()
        {
            _eventBus.Setup(x => x.SubscribeAsync(It.IsAny<IObserver<MockEvent>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));
            
            var observable = _eventBus.Object.GetObservable<MockEvent>();

            Assert.Throws<Exception>(() => observable.Subscribe());
        }

        [Fact]
        public void Subscribe_SubscribesActionCallback()
        {
            var subscription = _eventBus.Object.Subscribe<MockEvent>(_ => { });

            Assert.NotNull(subscription);
            _eventBus.Verify(x => x.SubscribeAsync(It.IsAny<IObserver<MockEvent>>(), _cancellationToken));
        }

        [Fact]
        public void Subscribe_SubscribesEventHandler()
        {
            var handler = _mocker.GetMock<IEventHandler<MockEvent>>();
            var expectedEvent = new MockEvent();
            IObserver<MockEvent>? observer = null;
            _eventBus.Setup(x => x.SubscribeAsync(It.IsAny<IObserver<MockEvent>>(), It.IsAny<CancellationToken>()))
                .Callback<IObserver<MockEvent>, CancellationToken>((callback, _) => observer = callback);

            var subscription = _eventBus.Object.Subscribe(handler.Object);

            Assert.NotNull(subscription);
            Assert.NotNull(observer);
            
            observer?.OnNext(expectedEvent);
            
            handler.Verify(x => x.HandleAsync(expectedEvent, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public void Subscribe_ThrowsWhenTypeIsNotEventType()
        {
            var handler = _mocker.Get<IEventHandler<MockEvent>>();

            Assert.Throws<InvalidOperationException>(
                () => _eventBus.Object.Subscribe(typeof(object), new[] { handler }));
        }

        [Fact]
        public void Subscribe_SubscribesNonGenericHandlerToEventType()
        {
            var handler = _mocker.GetMock<IEventHandler<MockEvent>>();
            var expectedEvent = new MockEvent();
            IObserver<MockEvent>? observer = null;
            _eventBus.Setup(x => x.SubscribeAsync(It.IsAny<IObserver<MockEvent>>(), It.IsAny<CancellationToken>()))
                .Callback<IObserver<MockEvent>, CancellationToken>((callback, _) => observer = callback);

            var subscriptions = _eventBus.Object.Subscribe(typeof(MockEvent), new[] { handler.Object });
            
            Assert.NotNull(subscriptions);
            Assert.Single(subscriptions);
            Assert.NotNull(observer);
            
            observer?.OnNext(expectedEvent);
            
            handler.Verify(x => x.HandleAsync(expectedEvent, It.IsAny<CancellationToken>()));
        }
    }
}
