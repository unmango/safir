using System.CommandLine;
using System.CommandLine.Invocation;

namespace Safir.CommandLine.Tests;

public class HandlerApplicationTests
{
    private readonly HandlerApplication _app;

    public HandlerApplicationTests()
    {
        _app = new HandlerApplication(new(), _ => new());
    }

    [Fact]
    public void GetContext_BuildsOnce()
    {
        var context = GetContext(new("test"));

        var first = _app.GetContext(context);
        var second = _app.GetContext(context);

        Assert.Same(first, second);
    }

    [Fact]
    public void GetContext_ThrowsWhenInvokedWithDifferentInvocationContexts()
    {
        var context1 = GetContext(new("test1"));
        var context2 = GetContext(new("test2"));
        _ = _app.GetContext(context1);

        Assert.Throws<InvalidOperationException>(() => _app.GetContext(context2));
    }

    // TODO: Invoke tests probably?

    private static InvocationContext GetContext(Command command)
    {
        var parseResult = command.Parse(string.Empty);
        return new(parseResult, Mock.Of<IConsole>());
    }
}
