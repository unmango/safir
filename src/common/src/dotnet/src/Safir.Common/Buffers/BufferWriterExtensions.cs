using System.Buffers;
using System.IO;

namespace Safir.Common.Buffers
{
    public static class BufferWriterExtensions
    {
        public static Stream AsStream(this IBufferWriter<byte> writer)
        {
            return new BufferWriterOwner(writer).AsStream();
        }
        
        public static Stream AsStream<T>(this T writer)
            where T : struct, IBufferWriter<byte>
        {
            return new BufferWriterStream<T>(writer);
        }
    }
}
