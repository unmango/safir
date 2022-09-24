using System.Reactive.Subjects;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using Safir.Common;
using Safir.Rpc.Abstractions;

namespace Safir.Rpc.Hosting;

/// <summary>
/// I'm pretty sure this fundamentally doesn't work due to being unable to
/// load different frameworks, but I didn't want to throw away the code.
/// </summary>
[PublicAPI]
public abstract class AssemblyLoadService : IManagedService
{
    private readonly Subject<string> _error = new();
    private readonly Subject<string> _output = new();
    private string? _assemblyPath;
    private Uri? _uri;
    private IHost? _host;

    public IObservable<string> Error => _error;

    public IObservable<string> Output => _output;

    public Uri Uri => _uri ?? throw new InvalidOperationException("Service must be started first");

    public async ValueTask StartAsync(IEnumerable<string>? args, CancellationToken cancellationToken = default)
    {
        _assemblyPath = await GetAssemblyPathAsync(_output.OnNext, _error.OnNext, cancellationToken);

        var loadContext = new ServiceAssemblyLoadContext(_assemblyPath);
        var entryAssembly = loadContext.LoadFromAssemblyPath(_assemblyPath);

        var programType = entryAssembly.GetType("Safir.Agent.Program")
                          ?? throw new InvalidOperationException("Unable to find entry type");
        var createHostBuilderMethod = programType.GetMethod("CreateHostBuilder", BindingFlags.Public | BindingFlags.Static)
                                      ?? throw new InvalidOperationException("Unable to find host builder method");

        var grpcPort = NetUtil.NextFreePort();
        _uri = new Uri($"http://127.0.0.1:{grpcPort}");

        var startupArgs = CreateStartupArgs(_uri.AbsoluteUri);
        var invokeResult = createHostBuilderMethod.Invoke(null, new object?[] { startupArgs });

        // Not currently working because two different versions of the abstractions assembly get loaded
        // Pretty sure this is the underlying reason https://learn.microsoft.com/en-us/dotnet/core/tutorials/creating-app-with-plugin-support#plugin-framework-references
        if (invokeResult is not IHostBuilder builder)
            throw new InvalidOperationException("Unable create host builder");

        _host = builder.Build();
    }

    public async ValueTask StopAsync(CancellationToken cancellationToken)
    {
        if (_host is not null)
            await _host.StopAsync(cancellationToken);
    }

    protected virtual IEnumerable<string> CreateStartupArgs(string url) => new[] { "--urls", Quote(url) };

    protected abstract Task<string> GetAssemblyPathAsync(
        Action<string> onOutput,
        Action<string> onError,
        CancellationToken cancellationToken);

    private static string Quote(string value) => $"\"{value}\"";
}
