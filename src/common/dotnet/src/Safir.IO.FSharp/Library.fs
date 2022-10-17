namespace Safir.IO.FSharp

open System
open System.IO
open System.Runtime.CompilerServices

[<Extension>]
module FileSystemWatcherExtensions =
    [<Extension>]
    let CreateChangedObservable(watcher: FileSystemWatcher): IObservable<FileSystemEventArgs> =
        watcher.Changed

    [<Extension>]
    let CreateCreatedObservable(watcher: FileSystemWatcher): IObservable<FileSystemEventArgs> =
        watcher.Created

    [<Extension>]
    let CreateDeletedObservable(watcher: FileSystemWatcher): IObservable<FileSystemEventArgs> =
        watcher.Deleted

    [<Extension>]
    let CreateErrorObservable(watcher: FileSystemWatcher): IObservable<ErrorEventArgs> =
        watcher.Error

    [<Extension>]
    let CreateRenamedObservable(watcher: FileSystemWatcher): IObservable<RenamedEventArgs> =
        watcher.Renamed
