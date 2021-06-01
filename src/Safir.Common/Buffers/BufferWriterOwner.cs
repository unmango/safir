using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Safir.Common.Buffers
{
    // https://github.com/Sergio0694/WindowsCommunityToolkit/blob/4600e429ba9e2b3f3d5662d495843f1387c14533/Microsoft.Toolkit.HighPerformance/Streams/Sources/IBufferWriterOwner.cs
    public readonly struct BufferWriterOwner : IBufferWriter<byte>
    {
        private readonly IBufferWriter<byte> _writer;

        public BufferWriterOwner(IBufferWriter<byte> writer)
        {
            _writer = writer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(int count)
        {
            _writer.Advance(count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            return _writer.GetMemory(sizeHint);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<byte> GetSpan(int sizeHint = 0)
        {
            return _writer.GetSpan(sizeHint);
        }
    }
}
