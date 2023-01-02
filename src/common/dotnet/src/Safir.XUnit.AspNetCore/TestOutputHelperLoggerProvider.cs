using System.Collections.Concurrent;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Safir.XUnit.AspNetCore;

[PublicAPI]
public sealed class TestOutputHelperLoggerProvider : ILoggerProvider
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly ConcurrentDictionary<string, TestOutputHelperLogger> _loggers = new();

    public TestOutputHelperLoggerProvider(ITestOutputHelper outputHelper) => _outputHelper = outputHelper;

    public ILogger CreateLogger(string categoryName)
        => _loggers.GetOrAdd(categoryName, static (_, outputHelper) => new(outputHelper), _outputHelper);

    public void Dispose() => _loggers.Clear();
}
