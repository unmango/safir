using Safir.IO.FSharp;

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

        Changed = _watcher.CreateChangedObservable();
        Created = _watcher.CreateCreatedObservable();
        Deleted = _watcher.CreateDeletedObservable();
        Renamed = _watcher.CreateRenamedObservable();
        Error = _watcher.CreateErrorObservable();
    }

    public IObservable<FileSystemEventArgs> Created { get; }

    public IObservable<FileSystemEventArgs> Changed { get; }

    public IObservable<FileSystemEventArgs> Deleted { get; }

    public IObservable<RenamedEventArgs> Renamed { get; }

    public IObservable<ErrorEventArgs> Error { get; }

    public void Dispose() => _watcher.Dispose();
}
