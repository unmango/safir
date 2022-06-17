module Safir.Agent.Queries.ListFiles

open System.IO
open Safir.Agent.Configuration
open Safir.Agent.Protos

let listFiles (DataDirectory dataDirectory) maxDepth enumerateFileSystemEntries getRelativePath =
    enumerateFileSystemEntries(dataDirectory, "*", EnumerationOptions(MaxRecursionDepth = maxDepth))
    |> Seq.map (getRelativePath dataDirectory)
    |> Seq.map (fun f -> FileSystemEntry(Path = f))
