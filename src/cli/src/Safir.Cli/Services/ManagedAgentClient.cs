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

    public ManagedAgent Agent { get; } = new DevelopmentAgent();

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

    public abstract class ManagedAgent
    {
        public Uri Uri { get; }

        public abstract Task StartAsync();

        public abstract Task StopAsync();
    }

    private sealed class DevelopmentAgent : ManagedAgent
    {
        private string? _sourceRoot;

        public override async Task StartAsync()
        {
            if (string.IsNullOrWhiteSpace(_sourceRoot)) {
                _sourceRoot = await GetGitRoot();
            }

            throw new NotImplementedException();
        }

        public string ProjectPath => Path.Combine(
            _sourceRoot ?? throw new InvalidOperationException("Source root has not been initialized"),
            "src",
            "agent",
            "src",
            "Safir.Agent");

        public override Task StopAsync()
        {
            throw new NotImplementedException();
        }

        private static async Task<string> GetGitRoot(CancellationToken cancellationToken = default)
        {
            var process = new Process {
                StartInfo = new("git", "rev-parse --show-toplevel") {
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                },
            };

            process.Start();

            var revParseOutput = await process.StandardOutput.ReadToEndAsync();
            return revParseOutput.Trim();
        }
    }
}
