using System;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Safir.Agent.Client;
using Safir.Agent.Protos;
using Safir.Protos;

namespace Safir.Cli.Services;

internal sealed class ManagedAgentClient : IAgentClient, IDisposable, IAsyncDisposable
{
    private readonly CancellationTokenRegistration _registration;

    public ManagedAgentClient(InvocationContext invocationContext)
    {
        _registration = invocationContext.GetCancellationToken().Register(state => {
            ((ManagedAgentClient)state!).Agent.StopAsync().GetAwaiter().GetResult();
        }, this);
    }

    public ManagedAgent Agent { get; }

    public FileSystem.FileSystemClient FileSystem { get; }

    public Host.HostClient Host { get; }

    public void Dispose()
    {
        _registration.Dispose();
        Agent.StopAsync().GetAwaiter().GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        await _registration.DisposeAsync();
        await Agent.StopAsync();
    }
}
