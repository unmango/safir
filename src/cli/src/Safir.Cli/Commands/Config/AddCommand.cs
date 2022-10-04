using System.CommandLine;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Options;
using Safir.Cli.Configuration;
using Safir.Cli.DependencyInjection;
using Safir.CommandLine;
using Safir.CommandLine.Generator;

namespace Safir.Cli.Commands.Config;

internal static class AddCommand
{
    public static readonly IHandlerBuilder Builder = new HandlerBuilder()
        .ConfigureAppConfiguration(builder => {
            builder.AddSafirCliDefault();
        })
        .ConfigureServices(services => {
            services.AddSafirCliCore();
            services.AddSafirOptions();
            services.AddLocalConfiguration();
        })
        .UseAddCommandAddCommandHandler();

    public static readonly Argument<string> ServiceArgument = new("service", "The service to add");

    public static readonly Argument<Uri> UriArgument = new Argument<Uri>("uri", "The URI of the service")
        .WithValidator(ValidateUri);

    public static readonly Command Value = Create();

    private static Command Create()
    {
        var command = new Command("add", "Add a Safir service to be used with the CLI") {
            ServiceArgument,
            UriArgument,
        };

        command.SetHandler(
            (handler, parseResult) => (Task)handler.Execute(parseResult),
            Bind.FromServiceProvider<AddCommandHandler>(),
            Bind.FromServiceProvider<ParseResult>());

        return command;
    }

    private static void ValidateUri(ArgumentResult result)
    {
        try {
            var uri = result.GetValueForArgument(UriArgument);
            _ = new Uri(uri.OriginalString);
        }
        catch (UriFormatException e) {
            result.ErrorMessage = e.Message;
        }
    }

    internal class AddCommandHandler
    {
        private readonly IConsole _console;
        private readonly IOptionsMonitor<SafirOptions> _options;
        private readonly IUserConfiguration _configuration;

        public AddCommandHandler(IConsole console, IOptionsMonitor<SafirOptions> options, IUserConfiguration configuration)
            => (_console, _options, _configuration) = (console, options, configuration);

        [CommandHandler]
        public async Task Execute(ParseResult parseResult, CancellationToken cancellationToken = default)
        {
            var service = parseResult.GetValueForArgument(ServiceArgument);

            if (_options.CurrentValue.Agents?.Any(x => NameEquals(x.Name, service)) ?? false) {
                _console.WriteLine($"Agent with name \"{service}\" is already configured");
                return;
            }

            var uri = parseResult.GetValueForArgument(UriArgument);

            await _configuration.UpdateAsync(x => x.Agents.Add(new(service, uri)), cancellationToken);

            _console.WriteLine($"Added agent \"{service}\"");
        }

        private static bool NameEquals(string first, string second)
            => first.Equals(second, StringComparison.OrdinalIgnoreCase);
    }
}
