using System.CommandLine;
using Safir.Cli.Commands.Media;

namespace Safir.Cli.Commands;

internal static class MediaCommand
{
    public static readonly Command Value = Create();

    private static Command Create()
    {
        var command = new Command("media", "Safir media operations") {
            ListCommand.Value,
        };

        return command;
    }
}
