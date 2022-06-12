module Safir.Agent.Queries.ListFiles

open Safir.Agent.Configuration.ConfigurationTypes
open Safir.Agent.Protos
open Safir.IO.FSharp

let listFiles (dataDirectory: DataDirectory) (enumerateFileSystemEntries: string -> seq<string>) =
    enumerateFileSystemEntries dataDirectory
    |> Seq.map (Path.getRelativePath dataDirectory)
    |> Seq.map (fun f -> FileSystemEntry(Path = f))
