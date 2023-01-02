using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using JetBrains.Annotations;

namespace Safir.Manager.Fixture;

[PublicAPI]
public sealed class SafirManagerConfiguration
{
    private const int ManagerPort = 6900;

    public SafirManagerConfiguration()
        : this("safir-manager:latest") { }

    public SafirManagerConfiguration(string image)
    {
        Image = image;
    }

    public string Image { get; set; }

    public int DefaultPort => ManagerPort;

    public int Port { get; set; }

    public IDictionary<string, string> Environments { get; } = new Dictionary<string, string>();

    public IWaitForContainerOS WaitStrategy
        => Wait.ForUnixContainer().UntilPortIsAvailable(ManagerPort);
}
