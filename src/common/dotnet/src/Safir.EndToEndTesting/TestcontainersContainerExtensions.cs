using DotNet.Testcontainers.Containers;
using JetBrains.Annotations;

namespace Safir.EndToEndTesting;

[PublicAPI]
public static class TestcontainersContainerExtensions
{
    // TODO: Better name than ExecFail?
    public static async Task ExecFailAsync(
        this ITestcontainersContainer container,
        IList<string> command,
        CancellationToken cancellationToken = default)
    {
        var result = await container.ExecAsync(command, cancellationToken);

        if (result.ExitCode > 0) {
            // TODO: Fail somehow. We don't have a dep on `Assert.Fail` in the library
        }
    }
}
