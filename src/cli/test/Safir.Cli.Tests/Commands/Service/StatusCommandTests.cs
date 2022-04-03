using System.Linq;
using Safir.Cli.Commands.Service;
using Xunit;

namespace Safir.Cli.Tests.Commands.Service;

public class StatusCommandTests
{
    [Fact]
    public void RequiresServiceOption()
    {
        var command = new RestartCommand();

        var options = command.Options.OfType<ServiceOption>();

        var option = Assert.Single(options);
        Assert.NotNull(option);
        Assert.True(option!.IsRequired);
    }
}