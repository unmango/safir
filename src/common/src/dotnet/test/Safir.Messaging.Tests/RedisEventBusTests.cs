using System;
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
        private readonly CancellationToken _cancellationToken = default;

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
        public async Task SubscribeAsync_ThrowsWhenConnectionFails()
        {
            var observer = _mocker.GetMock<IObserver<MockEvent>>();
            
            _connectionPool.Setup(x => x.GetConnectionAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new RedisException("Test exception"));

            await Assert.ThrowsAsync<EventBusException>(
                () => _eventBus.SubscribeAsync(observer.Object, _cancellationToken));
        }

        [Fact]
        public async Task SubscribeAsync_SubscribesCallback()
        {
            var observer = _mocker.GetMock<IObserver<MockEvent>>();
            
            var subscription = await _eventBus.SubscribeAsync(observer.Object, _cancellationToken);

            Assert.NotNull(subscription);
            _connection.Verify(x => x.GetSubscriber(It.IsAny<object>()));
            _subscriber.Verify(x => x.SubscribeAsync(
                It.IsAny<RedisChannel>(),
                It.IsAny<Action<RedisChannel, RedisValue>>(),
                It.IsAny<CommandFlags>()));
        }

        [Fact]
        public async Task PublishAsync_ThrowsWhenConnectionFails()
        {
            _connectionPool.Setup(x => x.GetConnectionAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new RedisException("Test exception"));

            await Assert.ThrowsAsync<EventBusException>(
                () => _eventBus.PublishAsync(new MockEvent(), _cancellationToken));
        }

        [Fact]
        public async Task PublishAsync_PublishesEvent()
        {
            var message = new MockEvent();

            await _eventBus.PublishAsync(message, _cancellationToken);

            _connection.Verify(x => x.GetSubscriber(It.IsAny<object>()));
            _subscriber.Verify(x => x.PublishAsync("MockEvent", It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()));
        }
    }
}
