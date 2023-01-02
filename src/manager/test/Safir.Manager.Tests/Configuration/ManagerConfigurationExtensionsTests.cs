namespace Safir.Manager.Tests.Configuration;

[Trait("Category", "Unit")]
public class ManagerConfigurationExtensionsTests
{
    [Fact]
    public void GetAgentOptions_ReturnsEmptyWhenNullConfiguration()
    {
        ManagerConfiguration? configuration = null;

        var options = configuration.GetAgentOptions();

        Assert.Empty(options);
    }

    [Fact]
    public void GetAgentOptions_ReturnsEmptyWhenNullAgents()
    {
        ManagerConfiguration configuration = new() {
            Agents = null,
        };

        var options = configuration.GetAgentOptions();

        Assert.Empty(options);
    }

    [Fact]
    public void GetAgentOptions_SkipsAgentsWithNoUri()
    {
        const string expectedName = "Test";
        ManagerConfiguration configuration = new() {
            Agents = new Dictionary<string, AgentConfiguration> {
                [expectedName] = new() {
                    Uri = "https://example.com",
                },
                ["TestNull"] = new() {
                    Uri = null,
                },
            },
        };

        var options = configuration.GetAgentOptions();

        var single = Assert.Single(options);
        Assert.Equal(expectedName, single.Name);
    }

    [Fact]
    public void GetAgentOptions_CreatesAgentOptions()
    {
        const string expectedName = "Test";
        const string expectedUri = "https://example.com/";
        ManagerConfiguration configuration = new() {
            Agents = new Dictionary<string, AgentConfiguration> {
                [expectedName] = new() {
                    Uri = expectedUri,
                },
            },
        };

        var options = configuration.GetAgentOptions();

        var single = Assert.Single(options);
        Assert.Equal(expectedName, single.Name);
        Assert.Equal(expectedUri, single.Uri.AbsoluteUri);
    }
}
