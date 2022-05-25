using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Safir.Cli.Configuration;
using Safir.Cli.DependencyInjection;
using Safir.CommandLine;

namespace Safir.Cli.Commands.Config;

internal static class AddCommand
{
    private static readonly IHandlerBuilder _builder = new HandlerBuilder()
        .ConfigureAppConfiguration(builder => {
            builder.AddSafirCliDefault();
        })
        .ConfigureServices(services => {
            services.AddSafirCliCore();
            services.AddSafirOptions();
            services.AddLocalConfiguration();
        })
        .ConfigureHandler<AddCommandHandler>((handler, context) => handler.Execute(context.ParseResult));

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

        // _builder.SetHandler<AddCommandHandler>(
        //     command,
        //     (handler, result) => handler.Execute(result));

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

            if (_options.CurrentValue.Agents?.Any(x => NameEquals(x.Name, service)) ?? false) {
                _console.WriteLine($"Agent with name \"{service}\" is already configured");
                return;
            }

            var uri = parseResult.GetValueForArgument(UriArgument);

            await _configuration.UpdateAsync(
                x => x.Agents.Add(new(service, uri)),
                CancellationToken.None);

            _console.WriteLine($"Added agent \"{service}\"");
        }

        private static bool NameEquals(string first, string second)
            => first.Equals(second, StringComparison.OrdinalIgnoreCase);
    }
}
