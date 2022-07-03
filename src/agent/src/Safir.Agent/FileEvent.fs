namespace Safir.Agent

open System.IO
open Safir.Agent.Protos

type FileEvent =
    | Changed of FileChanged
    | Created of FileCreated
    | Deleted of FileDeleted
    | Renamed of FileRenamed

module FileEvent =
    let mapChanged (e: FileSystemEventArgs) = FileChanged(Path = e.Name)
    let mapCreated (e: FileSystemEventArgs) = FileCreated(Path = e.Name)
    let mapDeleted (e: FileSystemEventArgs) = FileDeleted(Path = e.Name)

    let mapRenamed (e: RenamedEventArgs) =
        FileRenamed(Path = e.Name, OldPath = e.OldName)
