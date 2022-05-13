using System;
using System.CommandLine;
using System.Threading.Tasks;

namespace Safir.Cli.Commands.Config;

internal static class AddCommand
{
    public static Command Create()
    {
        var command = new Command("add", "Add a Safir service to be used with the CLI");

        command.SetHandler(CreateHandler());

        return command;
    }

    internal static Func<Task> CreateHandler()
    {
        return Handle;
    }

    internal static async Task Handle() { }
}
