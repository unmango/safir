using System.CommandLine;
using Safir.Cli.Commands.Config;

namespace Safir.Cli.Commands;

internal static class ConfigCommand
{
    public static Command Create()
    {
        var command = new Command("config", "Read or modify local configuration");

        command.AddCommand(AddCommand.Create());

        return command;
    }
}
