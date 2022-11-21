using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Safir.Agent.Protos;
using Safir.AspNetCore.Testing;
using Safir.Grpc;
using Safir.Manager.Protos;
using Safir.XUnit.AspNetCore;
using Xunit.Abstractions;

namespace Safir.Manager.IntegrationTests.Services;

[Trait("Category", "Integration")]
public class MediaServiceTestsGrpc : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Mock<FileSystem.FileSystemClient> _fileSystemClient = new();
    private readonly Media.MediaClient _client;
    private ManagerConfiguration _configuration = new();

    public MediaServiceTestsGrpc(WebApplicationFactory<Program> factory, ITestOutputHelper outputHelper)
    {
        var channel = factory
            .WithOptions(() => _configuration)
            .WithGrpcClient(_fileSystemClient)
            .WithLogger(outputHelper)
            .CreateChannel();

        _client = new Media.MediaClient(channel);
    }

    [Fact]
    public async Task List_ReturnsTestData()
    {
        _configuration = new() {
            Agents = new Dictionary<string, AgentConfiguration> {
                ["Test"] = new() {
                    Uri = "https://example.com",
                },
            },
        };
        _fileSystemClient.Setup(
                x => x.ListFiles(It.IsAny<Empty>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FileSystemEntry[] { new() { Path = "Test" } });

        var result = await _client.List(new Empty())
            .ResponseStream
            .ToListAsync();

        var item = Assert.Single(result);
        Assert.Equal("Test", item.Host);
        Assert.Equal("Test", item.Path);
    }
}
