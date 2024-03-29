using System.Buffers;

namespace Safir.Common;

public static class SerializerExtensions
{
    public static async ValueTask<ReadOnlyMemory<byte>> SerializeAsMemoryAsync<T>(
        this ISerializer serializer,
        T value,
        CancellationToken cancellationToken = default)
    {
        var writer = new ArrayBufferWriter<byte>();
        await serializer.SerializeAsync(writer, value, cancellationToken);
        return writer.WrittenMemory;
    }
}
