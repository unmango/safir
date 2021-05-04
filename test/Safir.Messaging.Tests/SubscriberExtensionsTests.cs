using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Safir.Messaging.Tests.Fakes;
using StackExchange.Redis;
using Xunit;

namespace Safir.Messaging.Tests
{
    // There isn't a good way to test the observable messages at this point
    public class SubscriberExtensionsTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<ISubscriber> _subscriber;

        public SubscriberExtensionsTests()
        {
            _subscriber = _mocker.GetMock<ISubscriber>();
        }

        [Fact]
        public async Task PublishAsync_PublishesToRedisSubscriber()
        {
            const string channel = "MockEvent";
            
            await _subscriber.Object.PublishAsync(channel, new MockEvent());
            
            _subscriber.Verify(x => x.PublishAsync(channel, It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()));
        }
    }
}
