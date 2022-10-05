namespace Safir.CommandLine;

internal sealed class DefaultHandlerLifetime : IHandlerLifetime
{
    public ValueTask WaitForStartAsync(CancellationToken cancellationToken) => new();

    public ValueTask StopAsync(CancellationToken cancellationToken) => new();
}
