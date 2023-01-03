using System.Text.Json;
using Safir.Manager.Protos;

namespace Safir.Cli.EndToEndTests.Commands.Media;

[Trait("Category", "EndToEnd")]
public class ListCommandTests : CliTestBase
{
    private readonly string _config;

    public ListCommandTests(CliFixture fixture)
        : base(fixture)
    {
        _config = $$"""
            {
                "Managers": [
                    {
                        "Name": "{{ManagerName}}",
                        "Uri": "{{ManagerContainer.InternalAddress}}",
                    }
                ]
            }
            """;
    }

    [Fact]
    public async Task List()
    {
        const string file = "Test.txt";
        await AgentContainer.CreateMediaFileAsync(file);

        var result = await CliBuilder
            .WithConfiguration("/config/config.json", _config)
            .ExecAsync("dotnet", "Safir.Cli.dll", "media", "list");

        var actual = JsonSerializer.Deserialize<MediaItem[]>(result);

        Assert.NotNull(actual);
        var item = Assert.Single(actual);
        Assert.Equal(AgentName, item.Host);
        Assert.Equal(file, item.Path);
    }
}
