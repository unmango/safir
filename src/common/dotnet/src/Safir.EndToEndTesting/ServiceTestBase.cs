using System.Collections.Immutable;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Grpc.Net.Client;
using JetBrains.Annotations;
using Safir.XUnit.AspNetCore;
using Xunit;
using Xunit.Abstractions;

namespace Safir.EndToEndTesting;

[PublicAPI]
public abstract class ServiceTestBase : IAsyncLifetime
{
    private const int InternalPort = 80;

    protected ServiceTestBase(ServiceFixtureBase service, ITestOutputHelper output)
        : this(service, output, ServiceTestConfiguration.NoOp) { }

    protected ServiceTestBase(ServiceFixtureBase service, ITestOutputHelper output, ConfigureServiceTest configure)
        : this(configure(new(service, output, ImmutableList<ConfigureBuilder>.Empty))) { }

    protected ServiceTestBase(ServiceTestConfiguration configuration)
    {
        var (service, output, configureActions) = configuration;
        TestcontainersSettings.Logger = new TestOutputHelperLogger(output);
        var builder = new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage(service.Image)
            .WithPortBinding(InternalPort, true)
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilPortIsAvailable(InternalPort))
            .WithOutputConsumer(new TestOutputHelperOutputConsumer(output));

        Container = configureActions
            .Aggregate(builder, (current, configure) => configure(current))
            .Build();
    }

    public Task InitializeAsync() => Container.StartAsync();

    public Task DisposeAsync() => Container.DisposeAsync().AsTask();

    protected ITestcontainersContainer Container { get; }

    protected HttpClient GetHttpClient()
    {
        var publicPort = Container.GetMappedPublicPort(InternalPort);
        return new HttpClient {
            BaseAddress = new Uri($"http://localhost:{publicPort}"),
        };
    }

    protected GrpcChannel CreateGrpcChannel()
    {
        var publicPort = Container.GetMappedPublicPort(InternalPort);
        return GrpcChannel.ForAddress($"http://localhost:{publicPort}");
    }
}
