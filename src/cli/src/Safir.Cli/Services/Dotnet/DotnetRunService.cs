using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Safir.Cli.Internal.Wrappers.Process;

namespace Safir.Cli.Services.Dotnet
{
    internal class DotnetRunService : DotnetService
    {
        public DotnetRunService(
            IProcessFactory processFactory,
            IOptions<ConfigOptions> config,
            ILogger<DotnetRunService> logger,
            IEnumerable<string> args)
            : base(processFactory, config, logger, DotnetCommand.Run, args)
        { }
    }
}
