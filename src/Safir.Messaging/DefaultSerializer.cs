using System;
using JetBrains.Annotations;
using MessagePack;
using MessagePack.Resolvers;

namespace Safir.Messaging
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public sealed class DefaultSerializer : ISerializer
    {
        private static readonly MessagePackSerializerOptions _options = ContractlessStandardResolver.Options;
        private static readonly Lazy<ISerializer> _instance = new(() => new DefaultSerializer());

        public static ISerializer Instance => _instance.Value;

        public byte[] Serialize<T>(T value) => MessagePackSerializer.Serialize(value, _options);

        public T Deserialize<T>(byte[] value) => MessagePackSerializer.Deserialize<T>(value, _options);
    }
}
