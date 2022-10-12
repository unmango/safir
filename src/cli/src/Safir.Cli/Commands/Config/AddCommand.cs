using System.CommandLine;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Options;
using Safir.Cli.Commands.Config.Add;
using Safir.Cli.Configuration;
using Safir.Cli.DependencyInjection;
using Safir.CommandLine;

namespace Safir.Cli.Commands.Config;

internal static class AddCommand
{
    public static readonly Argument<string> ServiceArgument = new("service", "The service to add");

    public static readonly Argument<Uri> UriArgument = new Argument<Uri>("uri", "The URI of the service")
        .WithValidator(ValidateUri);

    public static readonly Command Value = Create();

    public static IHandlerBuilder CreateBuilder() => new HandlerBuilder()
        .ConfigureAppConfiguration(builder => {
            builder.AddSafirCliDefault();
        })
        .ConfigureServices(services => {
            services.AddSafirCliCore();
            services.AddSafirOptions();
            services.AddLocalConfiguration();
        });

    private static Command Create()
    {
        var command = new Command("add", "Add a Safir service to be used with the CLI") {
            AgentCommand.Value,
            ManagerCommand.Value,
        };

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

    internal abstract class Handler
    {
        private readonly IConsole _console;
        private readonly IOptions<SafirOptions> _options;
        private readonly IUserConfiguration _configuration;

        protected Handler(IConsole console, IOptions<SafirOptions> options, IUserConfiguration configuration)
        {
            _console = console;
            _options = options;
            _configuration = configuration;
        }

        public async Task Execute(ParseResult parseResult, CancellationToken cancellationToken = default)
        {
            var service = parseResult.GetValueForArgument(ServiceArgument);

            if (GetServiceOptions(_options.Value).Any(x => NameEquals(x.Name, service))) {
                _console.WriteLine($"Service with name \"{service}\" is already configured");
                return;
            }

            var uri = parseResult.GetValueForArgument(UriArgument);

            await _configuration.UpdateAsync(x => AddService(x, service, uri), cancellationToken);

            _console.WriteLine($"Added service \"{service}\"");
        }

        protected abstract void AddService(LocalConfiguration configuration, string service, Uri uri);

        protected abstract IEnumerable<ServiceOptions> GetServiceOptions(SafirOptions options);

        private static bool NameEquals(string first, string second)
            => first.Equals(second, StringComparison.OrdinalIgnoreCase);
    }
}
