using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Safir.Grpc;
using Safir.Manager.Protos;
using Xunit.Abstractions;

namespace Safir.Manager.EndToEndTests.Services;

public class MediaServiceTestsGrpc : IClassFixture<ManagerServiceFixture>, IAsyncLifetime
{
    private readonly ITestcontainersContainer _container;
    private readonly Media.MediaClient _client;

    public MediaServiceTestsGrpc(ManagerServiceFixture service, ITestOutputHelper outputHelper)
    {
        TestcontainersSettings.Logger = new TestOutputLogger(outputHelper);
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

    [Fact(Skip = "TODO: It fails to build the image right now for some reason")]
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

    private class TestOutputLogger : ILogger
    {
        private readonly ITestOutputHelper _outputHelper;

        public TestOutputLogger(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            _outputHelper.WriteLine(formatter(state, exception));
        }
    }
}
