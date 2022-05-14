using System.CommandLine;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Safir.Cli.DependencyInjection;

namespace Safir.Cli.Commands.Config;

internal static class AddCommand
{
    private static readonly IBinderFactory _services = new ServiceCollection()
        .AddSafirCliCore()
        .BuildBinderFactory();

    public static Command Create()
    {
        var command = new Command("add", "Add a Safir service to be used with the CLI");

        command.SetHandler<IConsole>(Handle);

        return command;
    }

    internal static async Task Handle(IConsole console) { }
}
