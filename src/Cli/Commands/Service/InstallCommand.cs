using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
using Cli.Internal;
using Microsoft.Extensions.Options;

namespace Cli.Commands.Service
{
    internal class InstallCommand : Command
    {
        public InstallCommand() : base("install", "Installs the specified service(s)")
        {
            AddArgument(new ServiceArgument());
            AddOption(new Option(new[] { "--concurrent" }, "Install multiple services concurrently"));
            AddOption(new Option(new[] { "-d", "--directory" }, "Optional directory to install to"));
        }

        // ReSharper disable once ClassNeverInstantiated.Global
        public sealed class InstallHandler : ICommandHandler
        {
            private readonly IOptions<ServiceOptions> _options;
            private readonly IInstallationService _installer;

            public InstallHandler(IOptions<ServiceOptions> options, IInstallationService installer)
            {
                _options = options;
                _installer = installer;
            }
            
            public bool Concurrent { get; set; }
            
            public string? Directory { get; set; }

            public IEnumerable<string> Services { get; set; } = null!;
            
            public async Task<int> InvokeAsync(InvocationContext context)
            {
                var services = _options.Value.Join(
                    Services,
                    x => x.Key,
                    x => x,
                    (pair, _) => pair.Value,
                    StringComparer.OrdinalIgnoreCase);

                await _installer.InstallAsync(
                    services,
                    Concurrent,
                    Directory,
                    context.GetCancellationToken());

                return context.ResultCode;
            }
        }
    }
}
