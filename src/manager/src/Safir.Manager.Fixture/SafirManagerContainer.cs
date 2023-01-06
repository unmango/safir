using DotNet.Testcontainers.Configurations;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Safir.Common.V1Alpha1;
using Safir.Fixture;
using Safir.Manager.V1Alpha1;

namespace Safir.Manager.Fixture;

[PublicAPI]
public class SafirManagerContainer : SafirContainer
{
    protected SafirManagerContainer(ITestcontainersConfiguration configuration, ILogger logger)
        : base(configuration, logger) { }

    public HostService.HostServiceClient CreateHostClient() => new(CreateChannel());

    public MediaService.MediaServiceClient CreateMediaClient() => new(CreateChannel());
}
