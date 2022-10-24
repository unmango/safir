using Google.Protobuf.WellKnownTypes;
using Safir.Grpc;
using Xunit.Abstractions;

namespace Safir.Manager.EndToEndTests.Services;

[Collection(ManagerServiceCollection.Name)]
public class MediaServiceTestsGrpc : ManagerServiceTestBase
{
    public MediaServiceTestsGrpc(ManagerServiceFixture service, ITestOutputHelper output)
        : base(service, output) { }

    [Fact]
    public async Task List_ReturnsTestData()
    {
        var result = await GetMediaClient().List(new Empty())
            .ResponseStream
            .ToListAsync();

        var item = Assert.Single(result);
        Assert.Equal("Test", item.Host);
        Assert.Equal("Test", item.Path);
    }
}
