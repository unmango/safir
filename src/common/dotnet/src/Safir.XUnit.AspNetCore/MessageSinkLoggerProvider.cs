using System.Collections.Concurrent;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Safir.XUnit.AspNetCore;

[PublicAPI]
public sealed class MessageSinkLoggerProvider : ILoggerProvider
{
    private readonly IMessageSink _messageSink;
    private readonly ConcurrentDictionary<string, MessageSinkLogger> _loggers = new();

    public MessageSinkLoggerProvider(IMessageSink messageSink) => _messageSink = messageSink;

    public ILogger CreateLogger(string categoryName)
        => _loggers.GetOrAdd(categoryName, static (_, sink) => new(sink), _messageSink);

    public void Dispose() => _loggers.Clear();
}
