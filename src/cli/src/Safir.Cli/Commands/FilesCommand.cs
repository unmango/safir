using System.CommandLine;

namespace Safir.Cli.Commands;

internal static class FilesCommand
{
    public static readonly Command Value = Create();

    private static Command Create()
    {
        var command = new Command("files", "File operations");

        return command;
    }
}
