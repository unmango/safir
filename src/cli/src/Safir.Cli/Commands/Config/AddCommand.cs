using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Safir.Cli.Configuration;
using Safir.Cli.DependencyInjection;

namespace Safir.Cli.Commands.Config;

internal static class AddCommand
{
    private static readonly CommandBuilder _builder = CommandBuilder.Create()
        .Configure(builder => {
            builder.AddSafirCliDefault();
        })
        .ConfigureServices(services => {
            services.AddSafirCliCore();
            // services.AddSafirOptions(); // TODO: This will register a second call to bind, which for lists duplicates items
            services.AddLocalConfiguration();
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
        private readonly IOptionsMonitor<SafirOptions> _options;
        private readonly IUserConfiguration _configuration;

        public AddCommandHandler(
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

            await _configuration.UpdateAsync(x => x.Agents.Add(new(service, string.Empty)), CancellationToken.None);
        }
    }
}
