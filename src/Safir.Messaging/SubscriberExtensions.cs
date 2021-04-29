using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ProtoBuf;
using StackExchange.Redis;

namespace Safir.Messaging
{
    public static class SubscriberExtensions
    {
        public static IObservable<T> CreateObservable<T>(this ISubscriber subscriber, RedisChannel channel)
        {
            return Observable.Create<T>(observer => subscriber.SubscribeAsync(channel, (_, value) => {
                using var stream = new MemoryStream(value);
                var message = Serializer.Deserialize<T>(stream);
                observer.OnNext(message);
            }));
        }
        
        public static IObservable<T> AsObservable<T>(this ChannelMessageQueue queue)
        {
            return Observable.Create<T>(observer => () => {
                queue.OnMessage(channelMessage => {
                    using var stream = new MemoryStream(channelMessage.Message);
                    var message = Serializer.Deserialize<T>(stream);
                    observer.OnNext(message);
                });
            });
        }

        public static Task PublishAsync<T>(this ISubscriber subscriber, RedisChannel channel, T message)
        {
            using var stream = new MemoryStream();
            Serializer.Serialize(stream, message);
            return subscriber.PublishAsync(channel, stream.ToArray());
        }
    }
}
