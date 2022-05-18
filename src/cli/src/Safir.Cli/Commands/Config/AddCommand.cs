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
        private readonly ILocalConfiguration _configuration;

        public AddCommandHandler(
            IConsole console,
            IOptionsMonitor<SafirOptions> options,
            ILocalConfiguration configuration)
        {
            _console = console;
            _options = options;
            _configuration = configuration;
        }

        public async Task Execute(ParseResult parseResult)
        {
            var service = parseResult.GetValueForArgument(ServiceArgument);

            await using var optionsStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(optionsStream, _options.CurrentValue);
            optionsStream.Position = 0;
            var optionsJson = await new StreamReader(optionsStream).ReadToEndAsync();
            _console.WriteLine(optionsJson);

            await _configuration.UpdateAsync(x => x.Agents.Add(new() { Name = service }), CancellationToken.None);

            await using var optionsStream2 = new MemoryStream();
            await JsonSerializer.SerializeAsync(optionsStream2, _options.CurrentValue);
            optionsStream2.Position = 0;
            var optionsJson2 = await new StreamReader(optionsStream2).ReadToEndAsync();
            _console.WriteLine(optionsJson2);
        }
    }
}
