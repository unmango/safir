using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Cli.Services;

public abstract class ManagedAgent
{
    public async Task<Uri> StartAsync(
        Action<string>? onOutput = null,
        Action<string>? onError = null,
        CancellationToken cancellationToken = default)
    {
        var assemblyPath = await GetAssemblyPathAsync(cancellationToken);
        var entryAssembly = Assembly.LoadFrom(assemblyPath);
        entryAssembly.CreateInstance();
    }

    public Task StopAsync(CancellationToken cancellationToken) { }

    protected abstract Task<string> GetAssemblyPathAsync(CancellationToken cancellationToken);
}
