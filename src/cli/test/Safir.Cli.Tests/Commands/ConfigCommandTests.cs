using Safir.Cli.Commands;
using Xunit;

namespace Safir.Cli.Tests.Commands;

public class ConfigCommandTests
{
    [Fact]
    public void Value_UsesAddSubcommand()
    {
        var command = ConfigCommand.Value;

        Assert.Contains(command.Subcommands, x => x.Name == "add");
    }
}