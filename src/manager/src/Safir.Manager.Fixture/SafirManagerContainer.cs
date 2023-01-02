using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Safir.Fixture;
using Safir.Manager.Protos;
using Safir.Protos;

namespace Safir.Manager.Fixture;

[PublicAPI]
public class SafirManagerContainer : SafirContainer
{
    protected SafirManagerContainer(ITestcontainersConfiguration configuration, ILogger logger)
        : base(configuration, logger) { }

    public Host.HostClient CreateHostClient() => new(CreateChannel());

    public Media.MediaClient CreateMediaClient() => new(CreateChannel());
}
