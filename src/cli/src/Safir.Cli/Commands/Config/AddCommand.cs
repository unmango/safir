using System.CommandLine;
using System.CommandLine.Parsing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Safir.Cli.Configuration;
using Safir.Cli.DependencyInjection;
using Safir.CommandLine;

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
            services.AddTransient<Handler>();
        });

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
            (handler, service, uri, cancellationToken) => handler.Execute(service, uri, cancellationToken),
            Bind.FromHandlerContext<Handler>(),
            ServiceArgument,
            UriArgument,
            Bind.CancellationToken());

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

    internal class Handler
    {
        private readonly IConsole _console;
        private readonly IOptionsMonitor<SafirOptions> _options;
        private readonly IUserConfiguration _configuration;

        public Handler(IConsole console, IOptionsMonitor<SafirOptions> options, IUserConfiguration configuration)
            => (_console, _options, _configuration) = (console, options, configuration);

        public async Task Execute(string service, Uri uri, CancellationToken cancellationToken = default)
        {
            if (_options.CurrentValue.Agents?.Any(x => NameEquals(x.Name, service)) ?? false) {
                _console.WriteLine($"Agent with name \"{service}\" is already configured");
                return;
            }

            await _configuration.UpdateAsync(x => x.Agents.Add(new(service, uri)), cancellationToken);

            _console.WriteLine($"Added agent \"{service}\"");
        }

        private static bool NameEquals(string first, string second)
            => first.Equals(second, StringComparison.OrdinalIgnoreCase);
    }
}
