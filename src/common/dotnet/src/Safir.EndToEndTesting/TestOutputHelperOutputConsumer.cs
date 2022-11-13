using System.Text;
using DotNet.Testcontainers.Configurations;
using Xunit.Abstractions;

namespace Safir.EndToEndTesting;

public sealed class TestOutputHelperOutputConsumer : IOutputConsumer
{
    public TestOutputHelperOutputConsumer(ITestOutputHelper outputHelper)
    {
        Stderr = new OutputHelperStream(outputHelper);
        Stdout = new OutputHelperStream(outputHelper);
    }

    public void Dispose() { }

    public bool Enabled => true;

    public Stream Stdout { get; }

    public Stream Stderr { get; }

    private class OutputHelperStream : Stream
    {
        private readonly ITestOutputHelper _outputHelper;

        public OutputHelperStream(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        public override void Flush() => throw new InvalidOperationException();

        public override int Read(byte[] buffer, int offset, int count) => throw new InvalidOperationException();

        public override long Seek(long offset, SeekOrigin origin) => throw new InvalidOperationException();

        public override void SetLength(long value) => throw new InvalidOperationException();

        public override void Write(byte[] buffer, int offset, int count)
        {
            var message = Encoding.UTF8.GetString(buffer, offset, count);

            if (string.IsNullOrWhiteSpace(message))
                return;

            _outputHelper.WriteLine(message);
        }

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => throw new InvalidOperationException();

        public override long Position
        {
            get => throw new InvalidOperationException();
            set => throw new InvalidOperationException();
        }
    }
}
