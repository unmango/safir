using JetBrains.Annotations;
using Safir.Fixture;

namespace Safir.Manager.Fixture;

[PublicAPI]
public sealed class SafirManagerConfiguration : SafirServiceConfiguration
{
    private const int ManagerPort = 6900;

    public SafirManagerConfiguration(int defaultPort = ManagerPort)
        : this("unstoppablemango/safir-manager:latest", defaultPort) { }

    public SafirManagerConfiguration(string image, int defaultPort = ManagerPort)
        : base(image, defaultPort) { }
}
