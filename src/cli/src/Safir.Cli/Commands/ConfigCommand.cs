using System.CommandLine;
using Safir.Cli.Commands.Config;
using Safir.CommandLine;

namespace Safir.Cli.Commands;

internal static class ConfigCommand
{
    public static readonly IReadOnlyDictionary<Command, IHandlerBuilder> CommandHandlers =
        new Dictionary<Command, IHandlerBuilder> {
            [AddCommand.Value] = AddCommand.Builder,
            [RemoveCommand.Value] = RemoveCommand.Builder,
        };

    public static readonly Command Value = Create();

    private static Command Create()
    {
        var command = new Command("config", "Read or modify local configuration");

        command.AddCommand(AddCommand.Value);
        command.AddCommand(RemoveCommand.Value);

        return command;
    }
}
