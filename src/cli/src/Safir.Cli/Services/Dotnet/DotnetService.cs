using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Safir.Cli.Internal.Wrappers.Process;
using Safir.Cli.Services.Process;

namespace Safir.Cli.Services.Dotnet
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
