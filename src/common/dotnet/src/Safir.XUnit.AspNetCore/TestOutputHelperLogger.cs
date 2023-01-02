using Microsoft.Extensions.Logging;
using Safir.Common;
using Xunit.Abstractions;

namespace Safir.XUnit.AspNetCore;

public class TestOutputHelperLogger : ILogger
{
    private readonly ITestOutputHelper _outputHelper;

    public TestOutputHelperLogger(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
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
        _outputHelper.WriteLine(message);
    }
}
