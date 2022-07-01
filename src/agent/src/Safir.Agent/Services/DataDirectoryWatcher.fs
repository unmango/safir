namespace Safir.Agent.Services

open System.IO
open System.IO.Abstractions
open System.Threading.Tasks
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

    member this.createObservablesFromEvents() =
        ()

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
            |> function
                | Ok x -> x
                | Error e ->
                    logger.LogInformation(Validation.getMessage e)
                    Task.CompletedTask

        member this.StopAsync(cancellationToken) = failwith "todo"
