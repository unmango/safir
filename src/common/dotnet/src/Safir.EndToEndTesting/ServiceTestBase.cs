using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Grpc.Net.Client;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Safir.EndToEndTesting;

[PublicAPI]
public abstract class ServiceTestBase : IAsyncLifetime
{
    private const int InternalPort = 80;

    protected ServiceTestBase(ServiceFixtureBase service, ITestOutputHelper output)
    {
        TestcontainersSettings.Logger = new TestOutputHelperLogger(output);
        Container = new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage(service.Image)
            .WithPortBinding(InternalPort, true)
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilPortIsAvailable(InternalPort))
            .WithOutputConsumer(new TestOutputHelperOutputConsumer(output))
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
