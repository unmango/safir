using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Safir.ExternalTools;

namespace Safir.Rpc.Hosting;

[PublicAPI]
public sealed class DevelopmentAssemblyLoadService : AssemblyLoadService
{
    private readonly string _relativePath;
    private readonly string _dll;

    public DevelopmentAssemblyLoadService(string relativePath, string dll)
    {
        _relativePath = relativePath;
        _dll = dll;
    }

    protected override async Task<string> GetAssemblyPathAsync(
        Action<string> onOutput,
        Action<string> onError,
        CancellationToken cancellationToken)
    {
        const string configuration = "Release";
        var gitRoot = await Git.GetRootAsync();
        var projectPath = Path.Combine(gitRoot, _relativePath);

        var rid = RuntimeInformation.RuntimeIdentifier;

        await Dotnet.Build(
            projectPath,
            configuration,
            // https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#enabledynamicloading
            // new[] { "/p:EnableDynamicLoading=true" },
            new[] { "--self-contained", "--runtime", rid },
            onOutput,
            cancellationToken);

        return Path.Combine(projectPath, "bin", configuration, "net6.0", rid, _dll);
    }
}
