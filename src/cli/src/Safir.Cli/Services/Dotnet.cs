using System;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Cli.Services;

internal static class Dotnet
{
    public static Task Build(
        string project,
        string configuration,
        Action<string> onOutput,
        CancellationToken cancellationToken = default)
        => CliTool.RunAsync(
            "dotnet",
            string.Join(' ', "build", project, "--configuration", configuration),
            onOutput,
            cancellationToken);
}
