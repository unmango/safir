module Safir.Agent.Queries.ListFiles

open System.IO.Abstractions
open Microsoft.Extensions.Options
open Safir.Agent.Configuration
open Safir.Agent.Protos
open Safir.Common.Reader
open Safir.IO.FSharp.Path

let listFiles =
    reader {
        let! (options: IOptions<AgentOptions>), (directory: IDirectory), (path: IPath) = ask

        let getDataDirectory =
            DataDirectory.get |> run directory

        let enumerate (DataDirectory x) =
            directory.EnumerateFileSystemEntries(x, "*")
            |> Seq.map (path.getRelativePath x)
            |> Seq.map (fun f -> FileSystemEntry(Path = f))

        return
            options.Value.DataDirectory
            |> (getDataDirectory >> Result.map enumerate)
    }

type ListFilesWrapper(options, directory, path) =
    member _.run = run (options, directory, path) listFiles
