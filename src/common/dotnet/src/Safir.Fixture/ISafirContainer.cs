using JetBrains.Annotations;

namespace Safir.Fixture;

[PublicAPI]
public interface ISafirContainer
{
    int ContainerPort { get; }

    int Port { get; }
}
