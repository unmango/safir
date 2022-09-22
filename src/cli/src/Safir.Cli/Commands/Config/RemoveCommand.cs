using System.CommandLine;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Options;
using Safir.Cli.Configuration;
using Safir.Cli.DependencyInjection;
using Safir.CommandLine;
using Safir.CommandLine.Generator;

namespace Safir.Cli.Commands.Config;

internal static class RemoveCommand
{
    private static readonly IHandlerBuilder _builder = HandlerBuilder.Create()
        .ConfigureAppConfiguration(builder => {
            builder.AddSafirCliDefault();
        })
        .ConfigureServices(services => {
            services.AddSafirCliCore();
            services.AddSafirOptions();
            services.AddLocalConfiguration();
        })
        .UseRemoveCommandRemoveCommandHandler();

    public static readonly Argument<string> ServiceArgument = new("service", "The service to remove");

    public static readonly Command Value = Create();

    private static Command Create()
    {
        var command = new Command("remove", "Remove a service from usage with the CLI") {
            ServiceArgument,
        };

        command.AddAlias("rm");
        command.SetHandler(_builder);

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

        [CommandHandler]
        public async Task Execute(ParseResult parseResult, CancellationToken cancellationToken = default)
        {
            var service = parseResult.GetValueForArgument(ServiceArgument);

            if (_options.CurrentValue.Agents?.All(x => !NameEquals(x.Name, service)) ?? true) {
                _console.WriteLine($"No service named \"{service}\" configured");
                return;
            }

            await _configuration.UpdateAsync(x => x.Agents.Remove(service), cancellationToken);

            _console.WriteLine($"Removed service \"{service}\"");
        }
    }

    private static void Remove(this ICollection<AgentConfiguration> agents, string service)
        => agents.Remove(agents.First(x => NameEquals(x.Name, service)));

    private static bool NameEquals(string first, string second)
        => first.Equals(second, StringComparison.OrdinalIgnoreCase);
}
