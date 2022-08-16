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
    let createWatcher =
        result {
            let! root =
                options.Value.DataDirectory
                |> Validation.dataDirectory directory.Exists

            logger.LogTrace("Creating filesystem watcher for {Root}", root)
            return Some(new FileSystemWatcher(root, EnableRaisingEvents = true, IncludeSubdirectories = true))
        }
        |> onError (fun e -> logger.LogInformation(Validation.getMessage e))
        |> defaultValue None

    let watcher: Lazy<FileSystemWatcher option> =
        lazy createWatcher

    interface IFileWatcher with
        member this.Changed =
            match watcher.Value with
            | None -> Observable.Empty<FileSystemEventArgs>()
            | Some w -> w.Changed

        member this.Created =
            match watcher.Value with
            | None -> Observable.Empty<FileSystemEventArgs>()
            | Some w -> w.Created

        member this.Deleted =
            match watcher.Value with
            | None -> Observable.Empty<FileSystemEventArgs>()
            | Some w -> w.Deleted

        member this.Error =
            match watcher.Value with
            | None -> Observable.Empty<ErrorEventArgs>()
            | Some w -> w.Error

        member this.Renamed =
            match watcher.Value with
            | None -> Observable.Empty<RenamedEventArgs>()
            | Some w -> w.Renamed

    interface IHostedService with
        member this.StartAsync _ =
            do logger.LogTrace("Ensuring watcher created")
            watcher.Value |> ignore
            Task.CompletedTask

        member this.StopAsync _ =
            do logger.LogInformation("Stopping data directory watcher")

            match watcher.Value with
            | None -> do logger.LogTrace("No file watcher to dispose")
            | Some w ->
                do logger.LogTrace("Disposing file watcher")
                do w.Dispose()

            do logger.LogTrace("Finishing data directory watcher cleanup")
            Task.CompletedTask
