using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core.Utils;
using Grpc.Net.Client;
using Safir.Manager.Protos;

namespace Safir.Manager.EndToEndTests.Services;

public class MediaServiceTestsGrpc : IClassFixture<ManagerServiceFixture>, IAsyncLifetime
{
    private readonly ITestcontainersContainer _container;
    private readonly Media.MediaClient _client;

    public MediaServiceTestsGrpc(ManagerServiceFixture service)
    {
        const int httpPort = 5000, httpsPort = 5001;

        _container = new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage(service.Image)
            .WithPortBinding(httpPort, true)
            .WithPortBinding(httpsPort, true)
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilPortIsAvailable(httpPort)
                .UntilPortIsAvailable(httpsPort))
            .Build();

        var channel = GrpcChannel.ForAddress($"http://localhost:{httpPort}");
        _client = new Media.MediaClient(channel);
    }

    [Fact]
    public async Task List_ReturnsTestData()
    {
        var result = await _client.List(new Empty())
            .ResponseStream
            .ToListAsync();

        var item = Assert.Single(result);
        Assert.Equal("Test", item.Host);
        Assert.Equal("Test", item.Path);
    }

    public Task InitializeAsync() => _container.StartAsync();

    public Task DisposeAsync() => _container.DisposeAsync().AsTask();
}
