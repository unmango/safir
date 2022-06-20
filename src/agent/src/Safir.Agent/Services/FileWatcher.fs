namespace Safir.Agent.Services

open System
open System.IO

type IFileWatcher =
    abstract member Created: IObservable<FileSystemEventArgs>
    abstract member Changed: IObservable<FileSystemEventArgs>
    abstract member Deleted: IObservable<FileSystemEventArgs>
    abstract member Renamed: IObservable<RenamedEventArgs>
    abstract member Error: IObservable<ErrorEventArgs>
