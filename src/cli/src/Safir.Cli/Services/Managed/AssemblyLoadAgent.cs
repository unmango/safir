using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Safir.Cli.Services.Managed;

internal abstract class AssemblyLoadAgent : IManagedAgent
{
    private string? _assemblyPath;
    private IHost? _host;

    public async Task<Uri> StartAsync(
        Action<string>? onOutput = null,
        Action<string>? onError = null,
        CancellationToken cancellationToken = default)
    {
        _assemblyPath = await GetAssemblyPathAsync(
            onOutput ?? (_ => { }),
            onError ?? (_ => { }),
            cancellationToken);

        var loadContext = new ServiceAssemblyLoadContext(_assemblyPath);
        var entryAssembly = loadContext.LoadFromAssemblyPath(_assemblyPath);

        var programType = entryAssembly.GetType("Safir.Agent.Program")
                          ?? throw new InvalidOperationException("Unable to find entry type");
        var createHostBuilderMethod = programType.GetMethod("CreateHostBuilder", BindingFlags.Public | BindingFlags.Static)
                                      ?? throw new InvalidOperationException("Unable to find host builder method");

        var grpcPort = NetUtil.NextFreePort();
        var uri = new Uri($"http://127.0.0.1:{grpcPort}");

        var startupArgs = new[] { "--urls", Quote(uri.AbsoluteUri) };
        var invokeResult = createHostBuilderMethod.Invoke(null, new object?[] { startupArgs });

        // Not currently working because two different versions of the abstractions assembly get loaded
        // Pretty sure this is the underlying reason https://learn.microsoft.com/en-us/dotnet/core/tutorials/creating-app-with-plugin-support#plugin-framework-references
        if (invokeResult is not IHostBuilder builder)
            throw new InvalidOperationException("Unable create host builder");

        _host = builder.Build();

        return uri;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_host is not null)
            await _host.StopAsync(cancellationToken);
    }

    protected abstract Task<string> GetAssemblyPathAsync(
        Action<string> onOutput,
        Action<string> onError,
        CancellationToken cancellationToken);

    private static string Quote(string value) => $"\"{value}\"";
}
