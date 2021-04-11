using System;
using System.IO;

namespace Safir.Agent.Services
{
    public interface IFileWatcher
    {
        IObservable<FileSystemEventArgs> Created { get; }
        
        IObservable<FileSystemEventArgs> Changed { get; }
        
        IObservable<FileSystemEventArgs> Deleted { get; }
        
        IObservable<RenamedEventArgs> Renamed { get; }
        
        IObservable<ErrorEventArgs> Error { get; }
    }
}
