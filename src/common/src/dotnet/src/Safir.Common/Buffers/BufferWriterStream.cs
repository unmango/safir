using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;

namespace Safir.Common.Buffers
{
    // https://github.com/Sergio0694/WindowsCommunityToolkit/blob/4600e429ba9e2b3f3d5662d495843f1387c14533/Microsoft.Toolkit.HighPerformance/Streams/IBufferWriterStream%7BTWriter%7D.cs
    public class BufferWriterStream<T> : Stream
        where T : struct, IBufferWriter<byte>
    {
        private const string NotSupportedMessage = "The requested operation is not supported by this stream";
        private T _writer;
        private bool _disposed;

        public BufferWriterStream(T writer)
        {
            _writer = writer;
        }

        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException(NotSupportedMessage);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException(NotSupportedMessage);
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException(NotSupportedMessage);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("source", "The current stream has already been disposed");
            }
            
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset can't be negative.");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count can't be negative.");
            }

            if (offset + count > buffer!.Length)
            {
                throw new ArgumentException(
                    "The sum of offset and count can't be larger than the buffer length.",
                    nameof(buffer));
            }

            var source = buffer.AsSpan(offset, count);
            var destination = _writer.GetSpan(count);

            if (!source.TryCopyTo(destination))
            {
                throw new ArgumentException("The current stream can't contain the requested input data.");
            }

            _writer.Advance(count);
        }

        public override bool CanRead => false;

        public override bool CanSeek => false;
        
        public override bool CanWrite
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => !_disposed;
        }

        public override long Length => throw new NotSupportedException(NotSupportedMessage);

        public override long Position
        {
            get => throw new NotSupportedException(NotSupportedMessage);
            set => throw new NotSupportedException(NotSupportedMessage);
        }

        protected override void Dispose(bool disposing)
        {
            _disposed = true;
        }
    }
}
