using System;
using System.IO;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using StackExchange.Redis;

namespace Safir.Messaging
{
    // TODO: Handle abstractions in generic args like IEvent
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class SubscriberExtensions
    {
        public static IObservable<T> AsObservable<T>(this ChannelMessageQueue queue)
        {
            return Observable.Create<T>(observer => () => {
                queue.OnMessage(channelMessage => {
                    observer.OnNext(Deserialize<T>(channelMessage.Message));
                });
            });
        }

        public static IObservable<T> CreateObservable<T>(this ISubscriber subscriber, RedisChannel channel)
        {
            return Observable.Create<T>(async observable => {
                var syncObservable = Observer.Synchronize(observable);
                await subscriber.SubscribeAsync(channel, (_, value) => {
                    syncObservable.OnNext(Deserialize<T>(value));
                });

                return Disposable.Create(channel, x => subscriber.Unsubscribe(x));
            });
        }

        public static Task<long> PublishAsync<T>(this ISubscriber subscriber, RedisChannel channel, T message)
        {
            return subscriber.PublishAsync(channel, Serialize(message));
        }

        public static async Task<IDisposable> SubscribeAsync<T>(
            this ISubscriber subscriber,
            RedisChannel channel,
            IObserver<T> observer)
        {
            await subscriber.SubscribeAsync(channel, (_, value) => {
                observer.OnNext(Deserialize<T>(value));
            });

            return Disposable.Create(channel, x => {
                subscriber.Unsubscribe(x);
                observer.OnCompleted();
            });
        }

        public static Task<IDisposable> SubscribeAsync<T>(
            this ISubscriber subscriber,
            RedisChannel channel,
            Action<T> callback)
        {
            return subscriber.SubscribeAsync(channel, Observer.Create(callback));
        }

        private static T Deserialize<T>(RedisValue value)
        {
            return DefaultSerializer.Instance.Deserialize<T>((byte[])value);
        }

        private static RedisValue Serialize<T>(T message)
        {
            using var stream = new MemoryStream();
            DefaultSerializer.Instance.Serialize(stream, message);
            return RedisValue.CreateFrom(stream);
        }
    }
}
