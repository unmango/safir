using System.CommandLine;
using Safir.Cli.Commands.Files;

namespace Safir.Cli.Commands;

internal static class FilesCommand
{
    public static readonly Command Value = Create();

    private static Command Create()
    {
        var command = new Command("files", "File operations");

        command.AddCommand(ListCommand.Value);

        return command;
    }
}
