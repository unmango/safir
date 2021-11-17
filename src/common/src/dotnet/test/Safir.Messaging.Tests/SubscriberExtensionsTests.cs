using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using MessagePack;
using MessagePack.Resolvers;
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
        public void CreateObservable_ConnectsObservable()
        {
            const string channel = "MockEvent";
            var subject = new Subject<MockEvent>();
            var subscriber = new FakeSubscriber(subject);
            var notification = new MockEvent();
            var flag = false;

            subscriber.CreateObservable<MockEvent>(channel).Subscribe(_ => flag = true);
            subject.OnNext(notification);
            
            Assert.True(flag);
        }

        [Fact]
        public async Task PublishAsync_PublishesToRedisSubscriber()
        {
            const string channel = "MockEvent";

            await _subscriber.Object.PublishAsync(channel, new MockEvent());

            _subscriber.Verify(x => x.PublishAsync(channel, It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()));
        }

        private class FakeSubscriber : MockSubscriberBase
        {
            private readonly ISubject<MockEvent> _subject;

            public FakeSubscriber(ISubject<MockEvent> subject)
            {
                _subject = subject;
            }

            public override Task SubscribeAsync(
                RedisChannel channel,
                Action<RedisChannel, RedisValue> handler,
                CommandFlags flags = CommandFlags.None)
            {
                _subject.Subscribe(x => {
                    var value = MessagePackSerializer.Serialize(x, ContractlessStandardResolver.Options);
                    handler(channel, value);
                });
                
                return Task.CompletedTask;
            }
        }
    }
}
