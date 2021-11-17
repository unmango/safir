using System;
using System.Reactive.Subjects;
using System.Threading;
using Moq;
using Moq.AutoMock;
using Safir.Messaging.Tests.Fakes;
using Xunit;

namespace Safir.Messaging.Tests
{
    public class ObservableEventHandlerExtensionsTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IEventHandler<MockEvent>> _handler;
        private readonly Subject<MockEvent> _subject = new();

        public ObservableEventHandlerExtensionsTests()
        {
            _handler = _mocker.GetMock<IEventHandler<MockEvent>>();
        }

        [Fact]
        public void ObserveWith_AddsHandlerToObservableStream()
        {
            var result = _subject.ObserveWith(_handler.Object);
            
            Assert.NotNull(result);

            var subscription = result.Subscribe();
            
            Assert.NotNull(subscription);
            Assert.True(_subject.HasObservers);
            var message = new MockEvent();
            
            _subject.OnNext(message);
            
            _handler.Verify(x => x.HandleAsync(message, It.IsAny<CancellationToken>()));
        }
    }
}
