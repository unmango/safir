using System.CommandLine;

namespace Safir.CommandLine.Tests;

public class CommandExtensionsTests
{
    private readonly Mock<IHandlerBuilder> _builder = new();
    private readonly Command _command = new("test");

    [Fact]
    public void CommandSetHandler_BuildsHandler()
    {
        _command.SetHandler(_builder.Object);

        _builder.Verify(x => x.Build());
    }

    [Fact]
    public void BuilderSetHandler_BuildsHandler()
    {
        _builder.Object.SetHandler(_command);

        _builder.Verify(x => x.Build());
    }
}
