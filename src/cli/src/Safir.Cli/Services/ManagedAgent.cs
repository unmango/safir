using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Safir.Cli.Services;

public abstract class ManagedAgent
{
    public Uri? Uri { get; protected set; }

    public abstract IDisposable OnError(Action<Process, DataReceivedEventArgs> callback);

    public abstract IDisposable OnOutput(Action<Process, DataReceivedEventArgs> callback);

    public abstract Task StartAsync();

    public abstract Task StopAsync();
}
