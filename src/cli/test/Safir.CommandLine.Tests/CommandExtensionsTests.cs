using System.CommandLine;

namespace Safir.CommandLine.Tests;

[Trait("Category", "Unit")]
public class CommandExtensionsTests
{
    private readonly Mock<IHandlerBuilder> _builder = new();
    private readonly Command _command = new("test");

    [Fact]
    public void SetHandler_BuildsHandler()
    {
        _command.SetHandler(_builder.Object);

        _builder.Verify(x => x.Build());
    }
}
