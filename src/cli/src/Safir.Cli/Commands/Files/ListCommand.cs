using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Safir.Cli.DependencyInjection;
using Safir.Cli.Services;
using Safir.Cli.Services.Managed;
using Safir.CommandLine;
using Safir.CommandLine.Generator;

namespace Safir.Cli.Commands.Files;

internal static class ListCommand
{
    private static readonly IHandlerBuilder _builder = new HandlerBuilder()
        .UseSafirDefaults()
        .AddConfiguredAgents()
        .ConfigureServices(services => {
            services.AddSingleton<IAgents, AgentClientManager>();
        })
        .UseListCommandHandler();

    public static readonly Command Value = Create();

    private static Command Create()
    {
        var command = new Command("list", "List files tracked by agents");

        _builder.SetHandler(command);

        return command;
    }

    [UsedImplicitly]
    internal class Handler
    {
        private readonly IAgents _agents;
        private readonly IConsole _console;
        private readonly ILogger<Handler> _logger;
        private IManagedAgent? _agent;

        public Handler(IAgents agents, IConsole console, ILogger<Handler> logger)
        {
            _agents = agents;
            _console = console;
            _logger = logger;
        }

        [CommandHandler]
        public async Task HandleAsync(ParseResult parseResult, CancellationToken cancellationToken)
        {
            // if (_agents.ShouldStartManagedAgent) {
            //     _agent = _agents.CreateManagedAgent();
            //     _agent.OnOutput((_, args) => _console.WriteLine(args.Data ?? string.Empty));
            //     await _agent.StartAsync();
            // }

            _agent = new DefaultManagedAgent();
            using var outSub = _agent.Output.Subscribe(_console.WriteLine);
            using var errSub = _agent.Error.Subscribe(_console.WriteLine);

            await _agent.StartAsync(null, cancellationToken);
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            await _agent.StopAsync(cancellationToken);

            // if (_agent is not null) {
            //     await _agent.StopAsync();
            // }
        }
    }
}
