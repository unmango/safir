using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Grpc.Net.Client;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Safir.Fixture;

[PublicAPI]
public abstract class SafirContainer : TestcontainersContainer, ISafirContainer
{
    private readonly string _internalHostname;

    protected SafirContainer(ITestcontainersConfiguration configuration, ILogger logger)
        : base(configuration, logger)
    {
        _internalHostname = configuration.Hostname;
    }

    public virtual int ContainerPort { get; set; }

    public int Port => GetMappedPublicPort(ContainerPort);

    public Uri InternalAddress => new UriBuilder("http", _internalHostname, ContainerPort).Uri;

    public Uri Address => new UriBuilder("http", Hostname, Port).Uri;

    public virtual GrpcChannel CreateChannel() => GrpcChannel.ForAddress(Address);

    public virtual HttpClient CreateHttpClient() => new() { BaseAddress = Address };
}
