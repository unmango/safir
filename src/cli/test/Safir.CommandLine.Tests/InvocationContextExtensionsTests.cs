using System.CommandLine;
using System.CommandLine.Invocation;

namespace Safir.CommandLine.Tests;

public class InvocationContextExtensionsTests
{
    [Fact]
    public void TryGetHandlerContext_ReturnsFalseWhenNoHandler()
    {
        var context = GetContext(new Command("test"));

        var result = context.TryGetHandlerContext(out var handlerContext);

        Assert.False(result);
        Assert.Null(handlerContext);
    }

    [Fact]
    public void TryGetHandlerContext_ReturnsFalseWhenNotHandlerApplication()
    {
        var command = new Command("test") {
            Handler = Mock.Of<ICommandHandler>(),
        };
        var context = GetContext(command);

        var result = context.TryGetHandlerContext(out var handlerContext);

        Assert.False(result);
        Assert.Null(handlerContext);
    }

    [Fact]
    public void TryGetHandlerContext_ReturnsTrueWhenHandlerApplication()
    {
        var command = new Command("test") {
            Handler = new HandlerApplication(new(), _ => new()),
        };
        var context = GetContext(command);

        var result = context.TryGetHandlerContext(out var handlerContext);

        Assert.True(result);
        Assert.NotNull(handlerContext);
    }

    private static InvocationContext GetContext(Command command)
    {
        var parseResult = command.Parse(string.Empty);
        return new(parseResult, Mock.Of<IConsole>());
    }
}
