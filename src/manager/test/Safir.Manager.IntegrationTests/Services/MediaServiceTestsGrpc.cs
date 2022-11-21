using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc.Testing;
using Safir.AspNetCore.Testing;
using Safir.Grpc;
using Safir.Manager.Protos;
using Safir.XUnit.AspNetCore;
using Xunit.Abstractions;

namespace Safir.Manager.IntegrationTests.Services;

[Trait("Category", "Integration")]
public class MediaServiceTestsGrpc : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Media.MediaClient _client;

    public MediaServiceTestsGrpc(WebApplicationFactory<Program> factory, ITestOutputHelper outputHelper)
    {
        var channel = factory
            .WithLogger(outputHelper)
            .CreateChannel();

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
