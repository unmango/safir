using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Safir.Common.ConnectionPool;
using Safir.Messaging.Tests.Fakes;
using StackExchange.Redis;
using Xunit;

namespace Safir.Messaging.Tests
{
    public class RedisEventBusTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IConnectionPool<IConnectionMultiplexer>> _connectionPool;
        private readonly Mock<IConnectionMultiplexer> _connection;
        private readonly Mock<ISubscriber> _subscriber;
        private readonly RedisEventBus _eventBus;

        public RedisEventBusTests()
        {
            _connectionPool = _mocker.GetMock<IConnectionPool<IConnectionMultiplexer>>();
            _connection = _mocker.GetMock<IConnectionMultiplexer>();
            _connectionPool.Setup(x => x.GetConnectionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(_connection.Object);
            _subscriber = _mocker.GetMock<ISubscriber>();
            _connection.Setup(x => x.GetSubscriber(It.IsAny<object>())).Returns(_subscriber.Object);

            _eventBus = _mocker.CreateInstance<RedisEventBus>();
        }

        [Fact]
        public void Constructor_StartsAConnection()
        {
            _connectionPool.Verify(x => x.GetConnectionAsync(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public void GetObservable_CreatesSubscriber()
        {
            _eventBus.GetObservable<MockEvent>();

            // TODO: Maybe some way to verify callback was subscribed? Seems impossible at the moment
            _connection.Verify(x => x.GetSubscriber(It.IsAny<object>()));
        }

        [Fact]
        public async Task PublishAsync_PublishesEvent()
        {
            var message = new MockEvent();

            await _eventBus.PublishAsync(message);
            
            _connection.Verify(x => x.GetSubscriber(It.IsAny<object>()));
            _subscriber.Verify(x => x.PublishAsync("MockEvent", It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()));
        }
    }
}
