namespace Safir.Agent.Services

open System.IO
open System.IO.Abstractions
open System.Reactive.Linq
open System.Threading.Tasks
open FSharp.Core
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Safir.Agent
open Safir.Agent.Configuration
open Safir.FSharp.Common

type DataDirectoryWatcher(options: IOptions<AgentOptions>, directory: IDirectory, logger: ILogger<DataDirectoryWatcher>) =
    let mutable watcher: FileSystemWatcher option =
        None

    interface IFileWatcher with
        member this.Changed =
            match watcher with
            | None -> Observable.Empty<FileSystemEventArgs>()
            | Some w -> w.Changed

        member this.Created =
            match watcher with
            | None -> Observable.Empty<FileSystemEventArgs>()
            | Some w -> w.Created

        member this.Deleted =
            match watcher with
            | None -> Observable.Empty<FileSystemEventArgs>()
            | Some w -> w.Deleted

        member this.Error =
            match watcher with
            | None -> Observable.Empty<ErrorEventArgs>()
            | Some w -> w.Error

        member this.Renamed =
            match watcher with
            | None -> Observable.Empty<RenamedEventArgs>()
            | Some w -> w.Renamed

    interface IHostedService with
        member this.StartAsync _ =
            result {
                let! root =
                    options.Value.DataDirectory
                    |> Validation.dataDirectory directory.Exists

                logger.LogTrace("Creating filesystem watcher")
                watcher <- Some(new FileSystemWatcher(root, EnableRaisingEvents = true, IncludeSubdirectories = true))

                return Task.CompletedTask
            }
            |> onError (fun e -> logger.LogInformation(Validation.getMessage e))
            |> defaultValue Task.CompletedTask

        member this.StopAsync _ =
            do logger.LogInformation("Stopping data directory watcher")

            match watcher with
            | None -> do logger.LogTrace("No file watcher to dispose")
            | Some w ->
                do logger.LogTrace("Disposing file watcher")
                do w.Dispose()

            do logger.LogTrace("Finishing data directory watcher cleanup")
            Task.CompletedTask
