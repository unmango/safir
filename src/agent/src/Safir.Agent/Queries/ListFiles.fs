module Safir.Agent.Queries.ListFiles

open System.IO.Abstractions
open Safir.Agent.Configuration.ConfigurationTypes
open Safir.Agent.Protos
open Safir.IO.FSharp.Path

let listFiles (dataDirectory: DataDirectory) (directory: IDirectory) (path: IPath) =
    directory.EnumerateFileSystemEntries(dataDirectory, "*")
    |> Seq.map (path.getRelativePath dataDirectory)
    |> Seq.map (fun f -> FileSystemEntry(Path = f))
