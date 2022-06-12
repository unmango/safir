module Safir.Agent.Queries.ListFiles

open Safir.Agent.Configuration
open Safir.Agent.Protos

type Response =
    | Files of seq<FileSystemEntry>
    | DataDirectoryNotConfigured
    | DataDirectoryDoesNotExist

let listFiles (dataDirectory: DataDirectory) enumerateFileSystemEntries getRelativePath =
    match dataDirectory with
    | Some root when directory.exists root ->
        enumerateFileSystemEntries root
        |> Seq.map (getRelativePath root)
        |> Seq.map (fun f -> FileSystemEntry(Path = f))
        |> Files
    | Some x when String.IsNullOrWhiteSpace x -> DataDirectoryNotConfigured
    | None -> DataDirectoryNotConfigured
    | _ -> DataDirectoryDoesNotExist
