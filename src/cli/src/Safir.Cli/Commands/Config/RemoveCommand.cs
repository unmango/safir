using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Safir.Cli.Configuration;
using Safir.Cli.DependencyInjection;

namespace Safir.Cli.Commands.Config;

internal static class RemoveCommand
{
    private static readonly CommandBuilder _builder = CommandBuilder.Create()
        .Configure(builder => {
            builder.AddSafirCliDefault();
        })
        .ConfigureServices(services => {
            services.AddSafirCliCore();
            services.AddSafirOptions();
            services.AddLocalConfiguration();
        });

    public static readonly Argument<string> ServiceArgument = new("service", "The service to remove");

    public static readonly Command Value = Create();

    private static Command Create()
    {
        var command = new Command("remove", "Remove a service from usage with the CLI") {
            ServiceArgument,
        };

        command.AddAlias("rm");

        _builder.SetHandler<RemoveCommandHandler>(
            command,
            (handler, result) => handler.Execute(result));

        return command;
    }

    internal class RemoveCommandHandler
    {
        private readonly IConsole _console;
        private readonly IOptionsMonitor<SafirOptions> _options;
        private readonly IUserConfiguration _configuration;

        public RemoveCommandHandler(
            IConsole console,
            IOptionsMonitor<SafirOptions> options,
            IUserConfiguration configuration)
        {
            _console = console;
            _options = options;
            _configuration = configuration;
        }

        public async Task Execute(ParseResult parseResult)
        {
            var service = parseResult.GetValueForArgument(ServiceArgument);

            if (_options.CurrentValue.Agents?.All(x => !NameEquals(x.Name, service)) ?? true) {
                _console.WriteLine($"No service named \"{service}\" configured");
                return;
            }

            await _configuration.UpdateAsync(
                x => x.Agents.Remove(service),
                CancellationToken.None);

            _console.WriteLine($"Removed service \"{service}\"");
        }
    }

    private static void Remove(this ICollection<AgentConfiguration> agents, string service)
    {
        agents.Remove(agents.First(x => NameEquals(x.Name, service)));
    }

    private static bool NameEquals(string first, string second)
        => first.Equals(second, StringComparison.CurrentCultureIgnoreCase);
}
