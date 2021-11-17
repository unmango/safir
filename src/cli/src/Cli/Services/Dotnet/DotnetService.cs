using System.Collections.Generic;
using Cli.Internal.Wrappers.Process;
using Cli.Services.Process;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cli.Services.Dotnet
{
    internal abstract class DotnetService : ProcessService
    {
        // Lots TODO
        protected DotnetService(
            IProcessFactory processFactory,
            IOptions<ConfigOptions> config,
            ILogger<DotnetService> logger,
            DotnetCommand dotnetCommand,
            IEnumerable<string> args)
            : base(processFactory, config, logger, dotnetCommand.GetCommand(), args)
        { }
    }
}
