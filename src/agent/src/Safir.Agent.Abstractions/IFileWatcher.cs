using System;
using System.IO;
using JetBrains.Annotations;

namespace Safir.Agent
{
    [PublicAPI]
    public interface IFileWatcher
    {
        IObservable<FileSystemEventArgs> Created { get; }
        
        IObservable<FileSystemEventArgs> Changed { get; }
        
        IObservable<FileSystemEventArgs> Deleted { get; }
        
        IObservable<RenamedEventArgs> Renamed { get; }
        
        IObservable<ErrorEventArgs> Error { get; }
    }
}
