module Safir.Agent.Queries.ListFiles

open Safir.Agent.Configuration.ConfigurationTypes
open Safir.Agent.Protos

type Response = Files of seq<FileSystemEntry>

let listFiles
    (dataDirectory: DataDirectory)
    (enumerateFileSystemEntries: string -> seq<string>)
    (getRelativePath: string -> string -> string)
    =
    enumerateFileSystemEntries dataDirectory
    |> Seq.map (getRelativePath dataDirectory)
    |> Seq.map (fun f -> FileSystemEntry(Path = f))
    |> Files
