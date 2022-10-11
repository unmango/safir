using System.CommandLine;
using Microsoft.Extensions.Options;
using Safir.Cli.Configuration;
using Safir.CommandLine;

namespace Safir.Cli.Commands.Config.Add;

internal static class ManagerCommand
{
    private static Command Create()
    {
        var command = new Command("manager", "Add a Safir manager service to be used with the CLI") {
            AddCommand.ServiceArgument,
            AddCommand.UriArgument,
        };

        var builder = AddCommand.CreateBuilder()
            .ConfigureHandler<Handler>((handler, result, cancellationToken)
                => handler.Execute(result, cancellationToken));

        command.SetHandler(builder);

        return command;
    }

    internal sealed class Handler : AddCommand.Handler
    {
        public Handler(IConsole console, IOptions<SafirOptions> options, IUserConfiguration configuration)
            : base(console, options, configuration) { }

        protected override void AddService(LocalConfiguration configuration, string service, Uri uri)
        {
            configuration.Managers.Add(new(service, uri));
        }

        protected override IEnumerable<ServiceOptions> GetServiceOptions(SafirOptions options)
        {
            return options.Managers ?? Enumerable.Empty<ServiceOptions>();
        }
    }
}
