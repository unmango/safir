using JetBrains.Annotations;
using Safir.Fixture;

namespace Safir.Agent.Fixture;

[PublicAPI]
public sealed class SafirAgentConfiguration : SafirServiceConfiguration
{
    private const int AgentPort = 6901;
    public const string DataDirectoryKey = "DataDirectory";
    public const string DefaultDataDirectory = "/data";

    public SafirAgentConfiguration(int defaultPort = AgentPort)
        : this("unstoppablemango/safir-agent:latest", defaultPort) { }

    public SafirAgentConfiguration(string image, int defaultPort = AgentPort)
        : base(image, defaultPort)
    {
        DataDirectory = DefaultDataDirectory;
    }

    public string DataDirectory
    {
        get => Environments[DataDirectoryKey];
        set => Environments[DataDirectoryKey] = value;
    }
}
