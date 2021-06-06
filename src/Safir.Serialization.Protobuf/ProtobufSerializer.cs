using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Safir.Common;

namespace Safir.Serialization.Protobuf
{
    public class ProtobufSerializer : ISerializer
    {
        public T Deserialize<T>(ReadOnlyMemory<byte> value)
        {
            var instance = Activator.CreateInstance<T>();
            AssertMessage(instance).MergeFrom(value.ToArray());
            return instance;
        }

        public ValueTask<T> DeserializeAsync<T>(ReadOnlyMemory<byte> value, CancellationToken cancellationToken = default)
        {
            var instance = Deserialize<T>(value);
            return new ValueTask<T>(instance);
        }

        public ValueTask<object> DeserializeAsync(
            Type type,
            ReadOnlyMemory<byte> value,
            CancellationToken cancellationToken = default)
        {
            var instance = Activator.CreateInstance(type);
            AssertMessage(instance).MergeFrom(value.ToArray());
            return new ValueTask<object>(instance);
        }

        public void Serialize<T>(IBufferWriter<byte> writer, T value)
        {
            AssertMessage(value).WriteTo(writer);
        }

        public void Serialize<T>(Stream stream, T value)
        {
            AssertMessage(value).WriteTo(stream);
        }

        public ValueTask SerializeAsync<T>(IBufferWriter<byte> writer, T value, CancellationToken cancellationToken = default)
        {
            Serialize(writer, value);
            return new ValueTask();
        }

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
