using Safir.Cli.Commands;
using Xunit;

namespace Safir.Cli.Tests.Commands;

[Trait("Category", "Unit")]
public class ConfigCommandTests
{
    [Fact]
    public void Value_UsesAddSubcommand()
    {
        var command = ConfigCommand.Value;

        Assert.Contains(command.Subcommands, x => x.Name == "add");
    }

    [Fact]
    public void Value_UsesRemoveSubcommand()
    {
        var command = ConfigCommand.Value;

        Assert.Contains(command.Subcommands, x => x.Name == "remove");
    }
}
