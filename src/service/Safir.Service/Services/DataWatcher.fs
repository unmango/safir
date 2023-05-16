namespace Safir.Service.Services

open System
open System.IO
open System.Threading.Tasks
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Safir.Service

type DataWatcher(config: IConfiguration, fileSystem: FileSystem.Service) =
    let mutable _watcher: FileSystemWatcher option = None
    let mutable subscriptions: IDisposable list = []

    interface IHostedService with
        member this.StartAsync(cancellationToken) =
            let watcher = new FileSystemWatcher(config["MediaDirectory"])

            subscriptions <- [
                watcher.Changed.Subscribe(fun e ->
                    fileSystem.Changed("yeet", e.FullPath, e.Name) |> Async.StartImmediate)
            ]

            _watcher <- Some watcher
            Task.CompletedTask

        member this.StopAsync(cancellationToken) = failwith "todo"
