using JetBrains.Annotations;

namespace Safir.CommandLine;

// Basically just
// https://github.com/dotnet/runtime/blob/d384ba94f4e1af4c41a83279bb2736984128b893/src/libraries/Microsoft.Extensions.Hosting.Abstractions/src/IHostLifetime.cs
// but scoped to handlers
[PublicAPI]
public interface IHandlerLifetime
{
    ValueTask WaitForStartAsync(CancellationToken cancellationToken);

    ValueTask StopAsync(CancellationToken cancellationToken);
}
