using System;
using System.Reactive.Subjects;
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
        private readonly Subject<MockEvent> _eventSubject = new();
        private readonly Mock<IEventBus> _eventBus;

        public EventBusExtensionsTests()
        {
            _eventBus = _mocker.GetMock<IEventBus>();
            _eventBus.Setup(x => x.GetObservable<MockEvent>()).Returns(_eventSubject);
        }

        [Fact]
        public void Subscribe_SubscribesActionCallback()
        {
            MockEvent? captured = null;
            Action<MockEvent> callback = x => captured = x;
            var expectedEvent = new MockEvent();

            _eventBus.Object.Subscribe(callback);

            _eventBus.Verify(x => x.GetObservable<MockEvent>());
            
            _eventSubject.OnNext(expectedEvent);
            
            Assert.Same(expectedEvent, captured);
        }

        [Fact]
        public void Subscribe_SubscribesEventHandler()
        {
            var handler = _mocker.GetMock<IEventHandler<MockEvent>>();
            var expectedEvent = new MockEvent();

            var subscription = _eventBus.Object.Subscribe(handler.Object);

            Assert.NotNull(subscription);
            _eventBus.Verify(x => x.GetObservable<MockEvent>());
            
            _eventSubject.OnNext(expectedEvent);
            
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

            var subscriptions = _eventBus.Object.Subscribe(typeof(MockEvent), new[] { handler.Object });
            
            Assert.NotNull(subscriptions);
            Assert.Single(subscriptions);
            _eventBus.Verify(x => x.GetObservable<MockEvent>());
            
            _eventSubject.OnNext(expectedEvent);
            
            handler.Verify(x => x.HandleAsync(expectedEvent, It.IsAny<CancellationToken>()));
        }
    }
}
