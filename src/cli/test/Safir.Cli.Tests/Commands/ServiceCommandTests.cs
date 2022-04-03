using System;
using System.Linq;
using Safir.Cli.Commands;
using Safir.Cli.Commands.Service;
using Xunit;

namespace Safir.Cli.Tests.Commands;

public class ServiceCommandTests
{
    [Theory]
    [InlineData(typeof(RestartCommand))]
    [InlineData(typeof(StartCommand))]
    [InlineData(typeof(StatusCommand))]
    [InlineData(typeof(StopCommand))]
    public void AddsSubCommands(Type commandType)
    {
        var command = new ServiceCommand();

        var subCommands = command.Children.Where(x => x.GetType() == commandType);

        var subCommand = Assert.Single(subCommands);
        Assert.NotNull(subCommand);
    }
}