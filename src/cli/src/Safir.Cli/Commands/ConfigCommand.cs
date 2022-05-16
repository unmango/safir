using System.CommandLine;
using Safir.Cli.Commands.Config;

namespace Safir.Cli.Commands;

internal static class ConfigCommand
{
    public static readonly Command Value = Create();

    private static Command Create()
    {
        var command = new Command("config", "Read or modify local configuration");

        command.AddCommand(AddCommand.Value);

        return command;
    }
}
