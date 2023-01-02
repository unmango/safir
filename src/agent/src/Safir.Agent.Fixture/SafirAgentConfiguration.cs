using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using JetBrains.Annotations;

namespace Safir.Agent.Fixture;

[PublicAPI]
public sealed class SafirAgentConfiguration
{
    private const int AgentPort = 6901;
    public const string DataDirectoryKey = "DataDirectory";
    public const string DefaultDataDirectory = "/data";

    public SafirAgentConfiguration()
        : this("safir-agent:latest") { }

    public SafirAgentConfiguration(string image)
    {
        Image = image;
        DataDirectory = DefaultDataDirectory;
    }

    public string Image { get; set; }

    public int DefaultPort => AgentPort;

    public int Port { get; set; }

    public IDictionary<string, string> Environments { get; } = new Dictionary<string, string>();

    public string DataDirectory
    {
        get => Environments[DataDirectoryKey];
        set => Environments[DataDirectoryKey] = value;
    }

    public IWaitForContainerOS WaitStrategy
        => Wait.ForUnixContainer().UntilPortIsAvailable(AgentPort);
}
