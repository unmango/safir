using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text.Json;
using Grpc.Core;
using Grpc.Core.Utils;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Safir.Cli.Configuration;
using Safir.Cli.DependencyInjection;
using Safir.CommandLine;
using MediaClient = Safir.Manager.Protos.Media.MediaClient;

namespace Safir.Cli.Commands.Media;

internal static class ListCommand
{
    private static readonly IHandlerBuilder _builder = HandlerBuilder.Create()
        .ConfigureAppConfiguration(builder => {
            builder.AddSafirCliDefault();
        })
        .ConfigureServices((context, services) => {
            services.AddSafirCliCore();
            services.AddSafirOptions();
            services.AddLocalConfiguration();

            var safirOptions = context.Configuration.Get<SafirOptions>();
            if (safirOptions.Managers is null)
                return;

            foreach (var manager in safirOptions.Managers) {
                services.AddGrpcClient<MediaClient>(manager.Name, options => {
                    options.Address = new(manager.Uri);
                    options.ChannelOptionsActions.Add(x => x.Credentials = ChannelCredentials.Insecure);
                });
            }
        })
        .ConfigureHandler<Handler>((handler, result, cancellationToken)
            => handler.Execute(result, cancellationToken));

    public static readonly Command Value = Create();

    private static Command Create()
    {
        var command = new Command("list", "List media");

        command.SetHandler(_builder);

        return command;
    }

    private sealed class Handler
    {
        private readonly IOptions<SafirOptions> _options;
        private readonly GrpcClientFactory _clientFactory;
        private readonly IConsole _console;

        public Handler(IOptions<SafirOptions> options, GrpcClientFactory clientFactory, IConsole console)
        {
            _options = options;
            _clientFactory = clientFactory;
            _console = console;
        }

        public async Task Execute(ParseResult parseResult, CancellationToken cancellationToken)
        {
            var managerOptions = _options.Value.Managers;
            if (managerOptions is null)
                throw new InvalidOperationException("No managers configured");

            var manager = managerOptions.First();
            var client = _clientFactory.CreateClient<MediaClient>(manager.Name);

            var results = await client.List(new(), cancellationToken: cancellationToken)
                .ResponseStream.ToListAsync();

            var output = JsonSerializer.Serialize(results, new JsonSerializerOptions {
                WriteIndented = true,
            });

            _console.WriteLine(output);
        }
    }
}
