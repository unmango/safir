module Safir.Agent.Queries.ListFiles

open System.IO
open Safir.Agent.Configuration
open Safir.Agent.Protos

let listFiles enumerateFileSystemEntries getRelativePath (DataDirectory dataDirectory) maxDepth =
    enumerateFileSystemEntries(dataDirectory, "*", EnumerationOptions(MaxRecursionDepth = maxDepth))
    |> Seq.map (fun f -> getRelativePath(dataDirectory, f))
    |> Seq.map (fun f -> FileSystemEntry(Path = f))
