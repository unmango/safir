using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Safir.Cli.EndToEndTests;

[UsedImplicitly]
public class CliContainer : TestcontainersContainer
{
    protected CliContainer(ITestcontainersConfiguration configuration, ILogger logger)
        : base(configuration, logger) { }
}
