using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Cli.Services.Managed;

internal sealed class DevelopmentManagedProcessAgent : ManagedProcessAgent
{
    protected override async Task<Process> StartProcessAsync(
        string url,
        Action<string> onOutput,
        Action<string> onError,
        CancellationToken cancellationToken)
    {
        var appStarted = new TaskCompletionSource();

        var projectPath = await AgentUtil.GetProjectPathAsync();
        var process = Dotnet.Run(
            projectPath,
            "Release",
            processArgs: AgentUtil.CreateStartupArgs(url),
            onOutput: data => {
                if (data.Contains("Now listening")) {
                    appStarted.SetResult();
                }

                onOutput(data);
            },
            onError: onError);

        var winner = await Task.WhenAny(
            Task.Delay(TimeSpan.FromSeconds(30), cancellationToken),
            appStarted.Task);

        if (winner != appStarted.Task)
            throw new InvalidOperationException("Agent didn't start in the allotted time");

        return process;
    }
}
