namespace Safir.Agent.Services

open System.IO
open System.IO.Abstractions
open System.Threading.Tasks
open FSharp.Core
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Safir.Agent
open Safir.Agent.Configuration
open Safir.FSharp.Common

type DataDirectoryWatcher(options: IOptions<AgentOptions>, directory: IDirectory, logger: ILogger<DataDirectoryWatcher>) =
    let mutable watcher: FileSystemWatcher =
        null

    interface IFileWatcher with
        member this.Changed = watcher.Changed
        member this.Created = watcher.Created
        member this.Deleted = watcher.Deleted
        member this.Error = watcher.Error
        member this.Renamed = watcher.Renamed

    interface IHostedService with
        member this.StartAsync _ =
            result {
                let! root =
                    options.Value.DataDirectory
                    |> Validation.dataDirectory directory.Exists

                logger.LogTrace("Creating filesystem watcher")
                watcher <- new FileSystemWatcher(root, EnableRaisingEvents = true, IncludeSubdirectories = true)

                return Task.CompletedTask
            }
            |> onError (fun e -> logger.LogInformation(Validation.getMessage e))
            |> defaultValue Task.CompletedTask

        member this.StopAsync _ =
            do logger.LogInformation("Stopping data directory watcher")

            if watcher = null then
                do logger.LogTrace("No file watcher to dispose")
            else
                do logger.LogTrace("Disposing file watcher")
                do watcher.Dispose()

            do logger.LogTrace("Finishing data directory watcher cleanup")
            Task.CompletedTask
