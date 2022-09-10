using System.CommandLine;
using System.CommandLine.Parsing;
using System.Threading;
using System.Threading.Tasks;
using Safir.Agent.Client.DependencyInjection;
using Safir.Cli.Configuration;
using Safir.Cli.DependencyInjection;
using Safir.Cli.Services;
using Safir.CommandLine;
using Safir.CommandLine.Generator;

namespace Safir.Cli.Commands.Files;

internal static class ListCommand
{
    private static readonly IHandlerBuilder _builder = new HandlerBuilder()
        .UseSafirDefaults()
        .AddConfiguredAgents()
        .UseListCommandHandler();

    public static readonly Command Value = Create();

    private static Command Create()
    {
        var command = new Command("list", "List files tracked by agents");

        _builder.SetHandler(command);

        return command;
    }

    internal class Handler
    {
        private readonly IAgents _agents;
        private readonly IConsole _console;

        public Handler(IAgents agents, IConsole console)
        {
            _agents = agents;
            _console = console;
        }

        [CommandHandler]
        public async Task HandleAsync(ParseResult parseResult, CancellationToken cancellationToken)
        {
        }
    }
}
