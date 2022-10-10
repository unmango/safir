using Google.Protobuf.WellKnownTypes;
using Grpc.Core.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Safir.AspNetCore.IntegrationTesting;
using Safir.Manager.Protos;

namespace Safir.Manager.IntegrationTests.Services;

public class MediaServiceTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Media.MediaClient _client;

    public MediaServiceTests(WebApplicationFactory<Program> factory)
    {
        var channel = factory.CreateChannel();
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
}
