using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Grpc.Net.Client;
using Safir.Manager.Protos;
using Safir.Protos;
using Xunit.Abstractions;

namespace Safir.Manager.EndToEndTests;

public abstract class ManagerServiceTestBase : IAsyncLifetime
{
    private const int InternalPort = 80;

    protected ManagerServiceTestBase(ManagerServiceFixture service, ITestOutputHelper output)
    {
        TestcontainersSettings.Logger = new TestOutputLogger(output);
        Container = new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage(service.Image)
            .WithPortBinding(InternalPort, true)
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilPortIsAvailable(InternalPort))
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

    protected Host.HostClient GetHostClient() => new(CreateChannel());

    protected Media.MediaClient GetMediaClient() => new(CreateChannel());

    private GrpcChannel CreateChannel()
    {
        var publicPort = Container.GetMappedPublicPort(InternalPort);
        return GrpcChannel.ForAddress($"http://localhost:{publicPort}");
    }
}
