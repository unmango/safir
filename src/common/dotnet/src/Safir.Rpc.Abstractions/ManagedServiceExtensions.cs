using JetBrains.Annotations;

namespace Safir.Rpc.Abstractions;

[PublicAPI]
public static class ManagedServiceExtensions
{
    public static ValueTask StartAsync(this IManagedService service, CancellationToken cancellationToken)
        => service.StartAsync(null, cancellationToken);
}
