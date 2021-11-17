using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using JetBrains.Annotations;
using Safir.Common;

namespace Safir.Serialization.Protobuf
{
    [PublicAPI]
    public class ProtobufSerializer : ISerializer
    {
        private static readonly Lazy<ProtobufSerializer> _instance = new();

        private static ISerializer Instance => _instance.Value;

        T ISerializer.Deserialize<T>(ReadOnlyMemory<byte> value)
        {
            var instance = Activator.CreateInstance<T>();
            AssertMessage(instance).MergeFrom(value.ToArray());
            return instance;
        }

        ValueTask<T> ISerializer.DeserializeAsync<T>(ReadOnlyMemory<byte> value, CancellationToken cancellationToken)
        {
            var instance = Deserialize<T>(value);
            return new ValueTask<T>(instance);
        }

        ValueTask<object> ISerializer.DeserializeAsync(Type type, ReadOnlyMemory<byte> value, CancellationToken cancellationToken)
        {
            var instance = Activator.CreateInstance(type);
            AssertMessage(instance).MergeFrom(value.ToArray());
            return new ValueTask<object>(instance);
        }

        void ISerializer.Serialize<T>(IBufferWriter<byte> writer, T value)
        {
            AssertMessage(value).WriteTo(writer);
        }

        void ISerializer.Serialize<T>(Stream stream, T value)
        {
            AssertMessage(value).WriteTo(stream);
        }

        ValueTask ISerializer.SerializeAsync<T>(IBufferWriter<byte> writer, T value, CancellationToken cancellationToken)
        {
            Serialize(writer, value);
            return new ValueTask();
        }

        public static T Deserialize<T>(ReadOnlyMemory<byte> value) => Instance.Deserialize<T>(value);

        public static ValueTask<T> DeserializeAsync<T>(ReadOnlyMemory<byte> value, CancellationToken cancellationToken = default)
            => Instance.DeserializeAsync<T>(value, cancellationToken);

        public static ValueTask<object> DeserializeAsync(
            Type type,
            ReadOnlyMemory<byte> value,
            CancellationToken cancellationToken = default)
            => Instance.DeserializeAsync(type, value, cancellationToken);

        public static void Serialize<T>(IBufferWriter<byte> writer, T value) => Instance.Serialize(writer, value);

        public static void Serialize<T>(Stream stream, T value) => Instance.Serialize(stream, value);

        public static ValueTask SerializeAsync<T>(
            IBufferWriter<byte> writer,
            T value,
            CancellationToken cancellationToken = default)
            => Instance.SerializeAsync(writer, value, cancellationToken);

        private static IMessage AssertMessage<T>(T value)
        {
            if (value is not IMessage message)
            {
                throw new NotSupportedException("Can't serialize value that does not implement IMessage");
            }

            return message;
        }
    }
}
