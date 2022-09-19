using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        => Build(project, configuration, Enumerable.Empty<string>(), onOutput, cancellationToken);

    public static Task Build(
        string project,
        string configuration,
        IEnumerable<string> extraArgs,
        Action<string>? onOutput = null,
        CancellationToken cancellationToken = default)
        => CliTool.RunAsync(
            "dotnet",
            string.Join(' ', new[] {
                "build", project,
                "--configuration", configuration,
            }.Concat(extraArgs)),
            onOutput ?? (_ => { }),
            cancellationToken);

    public static Process Run(
        string project,
        string configuration,
        IEnumerable<string> extraArgs,
        CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Don't let your dreams be memes");
}
