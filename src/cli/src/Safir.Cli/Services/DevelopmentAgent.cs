using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Cli.Services;

internal sealed class DevelopmentAgent : ManagedAgent
{
    protected override async Task<string> GetAssemblyPathAsync(CancellationToken cancellationToken)
    {
        const string configuration = "Release";
        var gitRoot = await Git.GetRootAsync();
        var projectPath = Path.Combine(gitRoot, "src", "agent", "src", "Safir.Agent");
        await Dotnet.Build(projectPath, configuration, Console.WriteLine, cancellationToken);
        return Path.Combine(projectPath, "bin", configuration, "net6.0", "Safir.Agent.dll");
    }
}
