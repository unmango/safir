using DotNet.Testcontainers.Configurations;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Safir.Fixture;
using Safir.Manager.V1Alpha1;

namespace Safir.Manager.Fixture;

[PublicAPI]
public class SafirManagerContainer : SafirContainer
{
    protected SafirManagerContainer(ITestcontainersConfiguration configuration, ILogger logger)
        : base(configuration, logger) { }

    public MediaService.MediaServiceClient CreateMediaClient() => new(CreateChannel());
}
