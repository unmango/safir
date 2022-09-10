using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Cli.Services;

internal sealed class DevelopmentAgent : ManagedAgent
{
    private readonly IList<DataReceivedEventHandler> _errorHandlers = new List<DataReceivedEventHandler>();
    private readonly IList<DataReceivedEventHandler> _outputHandlers = new List<DataReceivedEventHandler>();
    private string? _sourceRoot;
    private Process? _process;

    public override IDisposable OnError(Action<Process, DataReceivedEventArgs> callback)
    {
        void Handler(object sender, DataReceivedEventArgs args) => callback((Process)sender, args);
        _outputHandlers.Add(Handler);
        return new ProcessDataCallback(this, Handler);
    }

    public override IDisposable OnOutput(Action<Process, DataReceivedEventArgs> callback)
    {
        void Handler(object sender, DataReceivedEventArgs args) => callback((Process)sender, args);
        _outputHandlers.Add(Handler);
        return new ProcessDataCallback(this, Handler);
    }

    public override async Task StartAsync()
    {
        if (string.IsNullOrWhiteSpace(_sourceRoot)) {
            _sourceRoot = await Git.GetRootAsync();
        }

        var projectPath = GetProjectPath(_sourceRoot);

        var grpcPort = NetUtil.NextFreePort();
        var uri = $"http://127.0.0.1:{grpcPort}";
        Uri = new(uri);

        var builder = new StringBuilder();
        builder.AppendJoin(' ',
            "run",
            "--project",
            projectPath,
            "--",
            "--urls",
            $"\"{uri}\"");

        var enableOutputEvents = _outputHandlers.Any();
        var enableErrorEvents = _errorHandlers.Any();

        _process = new Process {
            StartInfo = {
                FileName = "dotnet",
                Arguments = builder.ToString(),
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
            },
            EnableRaisingEvents = enableOutputEvents || enableErrorEvents,
        };

        foreach (var handler in _outputHandlers) {
            _process.OutputDataReceived += handler;
        }

        foreach (var handler in _errorHandlers) {
            _process.ErrorDataReceived += handler;
        }

        var appStarted = new TaskCompletionSource();
        _process.OutputDataReceived += (_, args) => {
            if (args.Data?.Contains("Application started") ?? false) {
                appStarted.SetResult();
            }
        };

        _process.Start();

        if (enableOutputEvents)
            _process.BeginOutputReadLine();

        if (enableErrorEvents)
            _process.BeginErrorReadLine();

        var winner = await Task.WhenAny(
            Task.Delay(TimeSpan.FromSeconds(30)),
            appStarted.Task);

        if (winner != appStarted.Task)
            throw new InvalidOperationException("Agent didn't start in the allotted time");
    }

    public override async Task StopAsync()
    {
        if (_process is null) return;

        _process.Kill();

        var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        try {
            await _process.WaitForExitAsync(tokenSource.Token);
            _process.Close();
        }
        catch (TaskCanceledException e) {
            Console.WriteLine(e);
        }
    }

    private static string GetProjectPath(string sourceRoot) => Path.Combine(
        sourceRoot,
        "src",
        "agent",
        "src",
        "Safir.Agent");

    private class ProcessDataCallback : IDisposable
    {
        private readonly DevelopmentAgent _agent;
        private readonly DataReceivedEventHandler _handler;

        public ProcessDataCallback(DevelopmentAgent agent, DataReceivedEventHandler handler)
        {
            _agent = agent;
            _handler = handler;
        }

        public void Dispose()
        {
            if (_agent._process is null) return;

            _agent._process.OutputDataReceived -= _handler;
        }
    }
}
