using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Safir.Agent.Configuration;

namespace Safir.Agent.Services;

internal sealed class AnonymousPipeLifetime : BackgroundService, IHostLifetime
{
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IOptions<AgentOptions> _options;

    public AnonymousPipeLifetime(IHostApplicationLifetime applicationLifetime, IOptions<AgentOptions> options)
    {
        _applicationLifetime = applicationLifetime;
        _options = options;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var pipeHandle = _options.Value.PipeHandle;
        if (string.IsNullOrWhiteSpace(pipeHandle))
            throw new InvalidOperationException("Pipe handle is required for anonymous pipe lifetime");

        var stopRequested = false;
        await using PipeStream pipeStream = new AnonymousPipeClientStream(PipeDirection.In, pipeHandle);
        using var reader = new StreamReader(pipeStream);

        while (!stopRequested && !stoppingToken.IsCancellationRequested) {
            var message = await reader.ReadLineAsync();
            stopRequested = message == "stop";
        }

        _applicationLifetime.StopApplication();
    }

    public Task WaitForStartAsync(CancellationToken cancellationToken) => StartAsync(cancellationToken);
}
