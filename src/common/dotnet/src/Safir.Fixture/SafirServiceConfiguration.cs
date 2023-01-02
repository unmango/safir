using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using JetBrains.Annotations;

namespace Safir.Fixture;

[PublicAPI]
public abstract class SafirServiceConfiguration
{
    protected SafirServiceConfiguration(string image, int defaultPort)
    {
        Image = image;
        DefaultPort = defaultPort;
    }

    public string Image { get; set; }

    public int DefaultPort { get; }

    public int Port { get; set; }

    public IDictionary<string, string> Environments { get; } = new Dictionary<string, string>();

    public IWaitForContainerOS WaitStrategy => Wait.ForUnixContainer().UntilPortIsAvailable(DefaultPort);
}
