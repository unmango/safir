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
            CreateArgs(new[] {
                "build", project,
                "--configuration", configuration,
            }, extraArgs),
            onOutput ?? (_ => { }),
            cancellationToken);

    public static Process Run(
        string project,
        string configuration,
        IEnumerable<string>? dotnetArgs = null,
        IEnumerable<string>? processArgs = null,
        Action<string>? onOutput = null,
        Action<string>? onError = null)
        => CliTool.Start(
            "dotnet",
            CreateArgs(new[] {
                "run",
                "--project", project,
                "--configuration", configuration,
            }, dotnetArgs, processArgs),
            onOutput: onOutput,
            onError: onError,
            redirectStandardInput: true);

    private static string CreateArgs(
        IEnumerable<string> args,
        IEnumerable<string>? dotnetArgs,
        IEnumerable<string>? processArgs = null)
    {
        var toJoin = args;

        if (dotnetArgs is not null)
            toJoin = toJoin.Concat(dotnetArgs);

        if (processArgs is not null)
            toJoin = toJoin.Concat(processArgs.Prepend("--"));

        return string.Join(' ', toJoin);
    }
}
