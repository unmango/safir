using System.Linq;
using Moq;
using Moq.AutoMock;
using Safir.Messaging.Internal;
using Safir.Messaging.Tests.Fakes;
using Xunit;

namespace Safir.Messaging.Tests.Internal
{
    public class SubscribeHandlerWrapperExtensionsTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IEventBus> _eventBus;
        private readonly Mock<ISubscribeHandlerWrapper> _wrapper;

        public SubscribeHandlerWrapperExtensionsTests()
        {
            _eventBus = _mocker.GetMock<IEventBus>();
            _wrapper = _mocker.GetMock<ISubscribeHandlerWrapper>();
        }

        [Fact]
        public void Subscribe_SubscribesAllHandlers()
        {
            var handler1 = _mocker.GetMock<IEventHandler<MockEvent>>();
            var handler2 = _mocker.GetMock<IEventHandler<MockEvent>>();

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            _wrapper.Object.Subscribe(_eventBus.Object, new[] { handler1.Object, handler2.Object }).ToList();
            
            _wrapper.Verify(x => x.Subscribe(_eventBus.Object, handler1.Object));
            _wrapper.Verify(x => x.Subscribe(_eventBus.Object, handler2.Object));
        }
    }
}
