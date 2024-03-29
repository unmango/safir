using System.Text.Json;
using Safir.Manager.V1Alpha1;

namespace Safir.Cli.EndToEndTests.Commands.Media.ListCommandTests;

[Trait("Category", "EndToEnd")]
public class HappyPath : CliTestBase
{
    public HappyPath(CliFixture fixture)
        : base(fixture) { }

    [Fact]
    public async Task List_HappyPath()
    {
        const string file = "Test.txt";
        await AgentContainer.CreateMediaFileAsync(file);

        var result = await CliBuilder
            .WithConfiguration(ManagerConfig)
            .ExecCliAsync("media", "list");

        if (result.ExitCode > 0)
            Assert.Fail(result.Stderr);

        var actual = JsonSerializer.Deserialize<Response>(result.Stdout);
        Assert.NotNull(actual);

        var item = Assert.Single(actual.Media);
        Assert.Equal(AgentName, item.Host);
        Assert.Equal(file, item.Path);
    }

    private record Response(List<MediaItem> Media);
}
