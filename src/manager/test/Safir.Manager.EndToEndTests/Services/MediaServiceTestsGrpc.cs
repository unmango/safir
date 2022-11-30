using Google.Protobuf.WellKnownTypes;
using Safir.EndToEndTesting;
using Safir.Grpc;
using Xunit.Abstractions;

namespace Safir.Manager.EndToEndTests.Services;

[Collection(ManagerServiceCollection.Name)]
[Trait("Category", "EndToEnd")]
public class MediaServiceTestsGrpc : ManagerServiceTestBase
{
    private const string DataDirectory = "/data";

    private static readonly ConfigureServiceTest _configureTest =
        configuration => configuration
            .WithEnvironment("DataDirectory", DataDirectory);

    public MediaServiceTestsGrpc(ManagerServiceFixture service, ITestOutputHelper output)
        : base(service, output, _configureTest) { }

    [Fact]
    public async Task List_ReturnsTestData()
    {
        const string fileName = "Test.txt";
        var execResult = await Container.ExecAsync(new List<string> {
            "touch", Path.Combine(DataDirectory, fileName),
        });

        if (execResult.ExitCode > 0) {
            Assert.Fail(execResult.Stderr);
        }

        var result = await GetMediaClient().List(new Empty())
            .ResponseStream
            .ToListAsync();

        var item = Assert.Single(result);
        Assert.Equal("Test", item.Host);
        Assert.Equal(fileName, item.Path);
    }
}
