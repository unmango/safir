using JetBrains.Annotations;

namespace Safir.Rpc.Hosting;

[PublicAPI]
public sealed class AnonymousPipeLifetimeOptions
{
    public string? PipeHandle { get; set; }
}
