using System;
using System.Net;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Safir.Messaging.Tests.Fakes
{
    internal class MockSubscriberBase : ISubscriber
    {
        public virtual Task<TimeSpan> PingAsync(CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public virtual bool TryWait(Task task)
        {
            throw new NotImplementedException();
        }

        public virtual void Wait(Task task)
        {
            throw new NotImplementedException();
        }

        public virtual T Wait<T>(Task<T> task)
        {
            throw new NotImplementedException();
        }

        public virtual void WaitAll(params Task[] tasks)
        {
            throw new NotImplementedException();
        }

        public virtual IConnectionMultiplexer Multiplexer { get; } = null!;

        public virtual TimeSpan Ping(CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public virtual EndPoint IdentifyEndpoint(RedisChannel channel, CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public virtual Task<EndPoint> IdentifyEndpointAsync(
            RedisChannel channel,
            CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsConnected(RedisChannel channel = new RedisChannel())
        {
            throw new NotImplementedException();
        }

        public virtual long Publish(RedisChannel channel, RedisValue message, CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public virtual Task<long> PublishAsync(
            RedisChannel channel,
            RedisValue message,
            CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public virtual void Subscribe(
            RedisChannel channel,
            Action<RedisChannel, RedisValue> handler,
            CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public virtual ChannelMessageQueue Subscribe(RedisChannel channel, CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public virtual Task SubscribeAsync(
            RedisChannel channel,
            Action<RedisChannel, RedisValue> handler,
            CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public virtual Task<ChannelMessageQueue> SubscribeAsync(
            RedisChannel channel,
            CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public virtual EndPoint SubscribedEndpoint(RedisChannel channel)
        {
            throw new NotImplementedException();
        }

        public virtual void Unsubscribe(
            RedisChannel channel,
            Action<RedisChannel, RedisValue>? handler = null,
            CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public virtual void UnsubscribeAll(CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public virtual Task UnsubscribeAllAsync(CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public virtual Task UnsubscribeAsync(
            RedisChannel channel,
            Action<RedisChannel, RedisValue>? handler = null,
            CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }
    }
}
