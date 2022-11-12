using Microsoft.Extensions.Logging;
using Safir.Common;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Safir.EndToEndTesting;

public class TestOutputLogger : ILogger
{
    private readonly Action<string> _write;

    public TestOutputLogger(ITestOutputHelper outputHelper)
    {
        _write = outputHelper.WriteLine;
    }

    public TestOutputLogger(IMessageSink sink)
    {
        _write = x => sink.OnMessage(new DiagnosticMessage(x));
    }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
        => Disposable.NoOp;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        _write(formatter(state, exception));
    }
}
