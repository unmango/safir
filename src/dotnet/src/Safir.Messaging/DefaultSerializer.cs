using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MessagePack;
using MessagePack.Resolvers;
using Safir.Common;

namespace Safir.Messaging
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public sealed class DefaultSerializer : ISerializer
    {
        private static readonly MessagePackSerializerOptions _options = ContractlessStandardResolver.Options;
        private static readonly Lazy<ISerializer> _instance = new(() => new DefaultSerializer());

        public static ISerializer Instance => _instance.Value;

        public T Deserialize<T>(ReadOnlyMemory<byte> value)
        {
            return MessagePackSerializer.Deserialize<T>(value.ToArray(), _options);
        }

        public ValueTask<T> DeserializeAsync<T>(ReadOnlyMemory<byte> value, CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream(value.ToArray());
            return MessagePackSerializer.DeserializeAsync<T>(stream, _options, cancellationToken);
        }

        public ValueTask<object> DeserializeAsync(Type type, ReadOnlyMemory<byte> value, CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream(value.ToArray());
            return MessagePackSerializer.DeserializeAsync(type, stream, _options, cancellationToken);
        }

        public void Serialize<T>(IBufferWriter<byte> writer, T value)
        {
            MessagePackSerializer.Serialize(writer, value, _options);
        }

        public void Serialize<T>(Stream stream, T value)
        {
            MessagePackSerializer.Serialize(stream, value, _options);
        }

        public ValueTask SerializeAsync<T>(IBufferWriter<byte> writer, T value, CancellationToken cancellationToken = default)
        {
            MessagePackSerializer.Serialize(writer, value, _options, cancellationToken);
            
            return new();
        }
    }
}
