using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Cli.Services.Managed;

internal sealed class DevelopmentAssemblyLoadAgent : AssemblyLoadAgent
{
    protected override async Task<string> GetAssemblyPathAsync(
        Action<string> onOutput,
        Action<string> onError,
        CancellationToken cancellationToken)
    {
        const string configuration = "Release";
        var projectPath = await AgentUtil.GetProjectPathAsync();

        var rid = RuntimeInformation.RuntimeIdentifier;

        await Dotnet.Build(
            projectPath,
            configuration,
            // https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#enabledynamicloading
            // new[] { "/p:EnableDynamicLoading=true" },
            new[] { "--self-contained", "--runtime", rid },
            onOutput,
            cancellationToken);

        return Path.Combine(projectPath, "bin", configuration, "net6.0", rid, "Safir.Agent.dll");
    }
}
