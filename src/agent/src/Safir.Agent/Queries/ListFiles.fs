module Safir.Agent.Queries.ListFiles

open System.IO
open System.IO.Abstractions
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Safir.Agent
open Safir.Agent.Configuration
open Safir.Agent.Protos
open Safir.FSharp.Common

let enumerationOptions m =
    EnumerationOptions(MaxRecursionDepth = m)

let fileSystemEntry f = FileSystemEntry(Path = f)

let execute maxDepth dataDirectory (directory: IDirectory) (path: IPath) =
    directory.EnumerateFileSystemEntries(dataDirectory, "*", enumerationOptions maxDepth)
    |> Seq.map (fun f -> path.GetRelativePath(dataDirectory, f))
    |> Seq.map fileSystemEntry

let listFiles enumerateFileSystemEntries getRelativePath dataDirectory maxDepth =
    enumerateFileSystemEntries (dataDirectory, "*", EnumerationOptions(MaxRecursionDepth = maxDepth))
    |> Seq.map (fun f -> getRelativePath (dataDirectory, f))
    |> Seq.map (fun f -> FileSystemEntry(Path = f))

type ListFiles(options: IOptions<AgentOptions>, directory: IDirectory, path: IPath, logger: ILogger<ListFiles>) =
    let getDataDirectory =
        Validation.dataDirectory directory.Exists

    let listFiles =
        listFiles directory.EnumerateFileSystemEntries path.GetRelativePath

    member _.Execute() =
        (options.Value.DataDirectory, options.Value.MaxDepth)
        |> fun (d, m) -> (getDataDirectory d, m)
        |> fun (d, m) -> (Result.map (fun x -> listFiles x m)) d
        |> onError (Validation.getMessage >> logger.LogInformation)
        |> function
            | Ok x -> x
            | Error _ -> []

    member _.Execute2() =
        directory.EnumerateFileSystemEntries(options.Value.DataDirectory, "*", EnumerationOptions())
        |> Seq.map (fun f -> FileSystemEntry(Path = f))
