using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Cli.Services.Managed;

internal sealed class DevelopmentManagedProcessAgent : ManagedProcessAgent
{
    protected override async Task<Process> StartProcessAsync(
        IEnumerable<string> args,
        Action<string> onOutput,
        Action<string> onError,
        CancellationToken cancellationToken)
    {
        var appStarted = new TaskCompletionSource();
        var projectPath = await AgentUtil.GetProjectPathAsync();

        var process = Dotnet.Run(
            projectPath,
            // "Release",
            "Debug",
            processArgs: args,
            onOutput: data => {
                if (data.Contains("Now listening")) {
                    appStarted.SetResult();
                }

                onOutput(data);
            },
            onError: onError);

        var winner = await Task.WhenAny(
            Task.Delay(TimeSpan.FromSeconds(10), cancellationToken),
            appStarted.Task);

        if (winner != appStarted.Task)
            throw new InvalidOperationException("Agent didn't start in the allotted time");

        return process;
    }
}
