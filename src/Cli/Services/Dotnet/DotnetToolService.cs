using System.Collections.Generic;
using Cli.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cli.Services.Dotnet
{
    internal class DotnetToolService : DotnetService
    {
        public DotnetToolService(
            IProcessFactory processFactory,
            IOptions<ConfigOptions> config,
            ILogger<DotnetToolService> logger,
            IEnumerable<string> args)
            : base(processFactory, config, logger, DotnetCommand.Tool, args)
        { }
    }
}
