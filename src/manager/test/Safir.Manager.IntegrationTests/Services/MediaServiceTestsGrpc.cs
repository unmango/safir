using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Safir.Agent.V1Alpha1;
using Safir.AspNetCore.Testing;
using Safir.Manager.V1Alpha1;
using Safir.XUnit.AspNetCore;
using Xunit.Abstractions;
using ListRequest = Safir.Agent.V1Alpha1.ListRequest;
using ListResponse = Safir.Agent.V1Alpha1.ListResponse;

namespace Safir.Manager.IntegrationTests.Services;

[Trait("Category", "Integration")]
public class MediaServiceTestsGrpc : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Mock<FilesService.FilesServiceClient> _fileSystemClient = new();
    private readonly MediaService.MediaServiceClient _client;
    private ManagerConfiguration _configuration = new();

    public MediaServiceTestsGrpc(WebApplicationFactory<Program> factory, ITestOutputHelper outputHelper)
    {
        var channel = factory
            .WithOptions(() => _configuration)
            .WithGrpcClient(_fileSystemClient)
            .WithLogger(outputHelper)
            .CreateChannel();

        _client = new(channel);
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
                x => x.List(It.IsAny<ListRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListResponse[] { new() { Path = "Test" } });

        var result = await _client.ListAsync(new());

        var item = Assert.Single(result.Media);
        Assert.Equal("Test", item.Host);
        Assert.Equal("Test", item.Path);
    }
}
