using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Safir.Common;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Safir.XUnit.AspNetCore;

[PublicAPI]
public class MessageSinkLogger : ILogger
{
    private readonly IMessageSink _sink;

    public MessageSinkLogger(IMessageSink sink)
    {
        _sink = sink;
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
        var message = formatter(state, exception);
        var sinkMessage = new DiagnosticMessage(message);
        _ = _sink.OnMessage(sinkMessage);
    }
}
