using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
using Cli.Services.Installation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cli.Commands.Service
{
    internal class InstallCommand : Command
    {
        private static readonly Option<bool> _concurrent = new(
            new[] { "--concurrent" },
            "Install multiple services concurrently");

        private static readonly Option<string> _directory = new(
            new[] { "-d", "--directory" },
            "Optional directory to install to");

        private static readonly ServiceArgument _services = new("The name of the service to install");

        public InstallCommand() : base("install", "Installs the specified service(s)")
        {
            AddOption(_concurrent);
            AddOption(_directory);
            AddArgument(_services);
        }

        // ReSharper disable once ClassNeverInstantiated.Global
        public sealed class InstallHandler : ICommandHandler
        {
            private readonly IOptions<ServiceOptions> _options;
            private readonly IInstallationService _installer;
            private readonly ILogger<InstallCommand> _logger;

            public InstallHandler(
                IOptions<ServiceOptions> options,
                IInstallationService installer,
                ILogger<InstallCommand> logger)
            {
                _options = options;
                _installer = installer;
                _logger = logger;
            }

            public async Task<int> InvokeAsync(InvocationContext context)
            {
                var parseResult = context.ParseResult;
                var concurrent = parseResult.ValueForOption(_concurrent);
                var directory = parseResult.ValueForOption(_directory);
                var services = parseResult.ValueForArgument(_services)!.ToList();

                _logger.BoolOption(nameof(concurrent), concurrent);
                _logger.Option(nameof(directory), directory!);
                _logger.Option(nameof(services), services!);

                var toInstall = _options.Value
                    .Join(
                        services!,
                        x => x.Key,
                        x => x,
                        (pair, _) => (Name: pair.Key, Service: pair.Value),
                        StringComparer.OrdinalIgnoreCase)
                    .ToList();

                var names = toInstall.Select(x => x.Name).ToList();
                _logger.ServicesToInstall(names);
                _logger.UnmatchedServices(services.Except(names, StringComparer.OrdinalIgnoreCase));

                try
                {
                    await _installer.InstallAsync(
                        toInstall.Select(x => x.Service),
                        concurrent,
                        directory,
                        context.GetCancellationToken());

                    _logger.InstallationSucceeded();

                    return context.ResultCode;
                }
                catch (Exception e)
                {
                    _logger.InstallationFailed(e);
                    return 1;
                }
            }
        }
    }
}
