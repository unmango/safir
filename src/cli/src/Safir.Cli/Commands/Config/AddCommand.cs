using System.CommandLine;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Cli.DependencyInjection;

namespace Safir.Cli.Commands.Config;

internal static class AddCommand
{
    private static readonly CommandBuilder _builder = CommandBuilder.Create()
        .ConfigureServices(services => {
            services.AddSafirCliCore();
        });

    public static readonly Argument<string> ServiceArgument = new("service", "The service to add");

    public static readonly Command Value = Create();

    private static Command Create()
    {
        var command = new Command("add", "Add a Safir service to be used with the CLI") {
            ServiceArgument,
        };

        _builder.SetHandler<AddCommandHandler>(
            command,
            (handler, result) => handler.Execute(result));

        return command;
    }

    [UsedImplicitly]
    private class AddCommandHandler
    {
        private readonly IConsole _console;

        public AddCommandHandler(IConsole console)
        {
            _console = console;
        }

        public Task Execute(ParseResult parseResult)
        {
            _console.WriteLine("TODO");
            return Task.CompletedTask;
        }
    }
}
