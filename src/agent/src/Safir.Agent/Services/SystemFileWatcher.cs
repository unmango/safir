using System.Reactive.Linq;

namespace Safir.Agent.Services;

internal sealed class SystemFileWatcher : IFileWatcher, IDisposable
{
    private readonly FileSystemWatcher _watcher;

    public SystemFileWatcher(FileWatcherOptions options)
    {
        _watcher = new FileSystemWatcher(options.Path) {
            EnableRaisingEvents = true,
            IncludeSubdirectories = true,
        };

        Created = Observable
            .FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                x => _watcher.Created += x,
                x => _watcher.Created -= x)
            .Select(x => x.EventArgs);

        Changed = Observable
            .FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                x => _watcher.Created += x,
                x => _watcher.Created -= x)
            .Select(x => x.EventArgs);

        Deleted = Observable
            .FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                x => _watcher.Created += x,
                x => _watcher.Created -= x)
            .Select(x => x.EventArgs);

        Renamed = Observable
            .FromEventPattern<RenamedEventHandler, RenamedEventArgs>(
                x => _watcher.Renamed += x,
                x => _watcher.Renamed -= x)
            .Select(x => x.EventArgs);

        Error = Observable
            .FromEventPattern<ErrorEventHandler, ErrorEventArgs>(
                x => _watcher.Error += x,
                x => _watcher.Error -= x)
            .Select(x => x.EventArgs);
    }

    public IObservable<FileSystemEventArgs> Created { get; }

    public IObservable<FileSystemEventArgs> Changed { get; }

    public IObservable<FileSystemEventArgs> Deleted { get; }

    public IObservable<RenamedEventArgs> Renamed { get; }

    public IObservable<ErrorEventArgs> Error { get; }

    public void Dispose() => _watcher.Dispose();
}
