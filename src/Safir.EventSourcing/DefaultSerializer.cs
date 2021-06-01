using System;
using System.Buffers;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Safir.Common;
using Safir.Common.Buffers;

namespace Safir.EventSourcing
{
    public class DefaultSerializer : ISerializer
    {
        private static readonly JsonSerializerOptions _options = new();
        private static readonly JsonWriterOptions _writerOptions = new();
        private static readonly Lazy<DefaultSerializer> _instance = new();

        public static ISerializer Instance => _instance.Value;

        public T Deserialize<T>(ReadOnlyMemory<byte> value)
        {
            return JsonSerializer.Deserialize<T>(value.Span, _options)!;
        }

        public ValueTask<T> DeserializeAsync<T>(ReadOnlyMemory<byte> value, CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream(value.Span.ToArray());
            return JsonSerializer.DeserializeAsync<T>(stream, _options, cancellationToken)!;
        }

        public ValueTask<object> DeserializeAsync(
            Type type,
            ReadOnlyMemory<byte> value,
            CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream(value.Span.ToArray());
            return JsonSerializer.DeserializeAsync(stream, type, _options, cancellationToken)!;
        }

        public void Serialize<T>(IBufferWriter<byte> writer, T value)
        {
            using var jsonWriter = new Utf8JsonWriter(writer, _writerOptions);
            JsonSerializer.Serialize(jsonWriter, value, _options);
        }

        public void Serialize<T>(Stream stream, T value)
        {
            using var writer = new Utf8JsonWriter(stream, _writerOptions);
            JsonSerializer.Serialize(writer, value, _options);
        }

        public ValueTask SerializeAsync<T>(
            IBufferWriter<byte> writer,
            T value,
            CancellationToken cancellationToken = default)
        {
            using var stream = writer.AsStream();
            var task = JsonSerializer.SerializeAsync(
                stream,
                value,
                _options,
                cancellationToken);
            
            return new(task);
        }
    }
}
