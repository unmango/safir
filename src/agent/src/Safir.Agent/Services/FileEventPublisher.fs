namespace Safir.Agent.Services

open System
open System.IO
open System.Threading.Tasks
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Safir.Agent.Protos
open Safir.Agent.Services
open System.Reactive.Linq
open Safir.Messaging

type private Events =
    | Changed of FileChanged
    | Created of FileCreated
    | Deleted of FileDeleted
    | Renamed of FileRenamed

type FileEventPublisher(watcher: IFileWatcher, bus: IEventBus, logger: ILogger<FileEventPublisher>) =
    let mutable subscriptions: seq<IDisposable> =
        Seq.empty

    let mapChanged (e: FileSystemEventArgs) = FileChanged(Path = e.Name)
    let mapCreated (e: FileSystemEventArgs) = FileCreated(Path = e.Name)
    let mapDeleted (e: FileSystemEventArgs) = FileDeleted(Path = e.Name)

    let mapRenamed (e: RenamedEventArgs) =
        FileRenamed(Path = e.Name, OldPath = e.OldName)

    let logPublishing =
        function
        | Changed _ -> "changed"
        | Created _ -> "created"
        | Deleted _ -> "deleted"
        | Renamed _ -> "renamed"

    let tryPublish e cancellationToken =
        task {
            try
                do! bus.PublishAsync(e, cancellationToken)
            with
            | e -> logger.LogError(e, "Unable to publish event to bus")
        }

    interface IHostedService with
        member this.StartAsync _ =
            do logger.LogInformation("Starting file event publisher")

            subscriptions <-
                [ watcher
                      .Changed
                      .SelectMany(fun e c -> tryPublish (mapChanged e) c)
                      .Subscribe()
                  watcher
                      .Created
                      .SelectMany(fun e c -> tryPublish (mapCreated e) c)
                      .Subscribe()
                  watcher
                      .Deleted
                      .SelectMany(fun e c -> tryPublish (mapDeleted e) c)
                      .Subscribe()
                  watcher
                      .Renamed
                      .SelectMany(fun e c -> tryPublish (mapRenamed e) c)
                      .Subscribe() ]

            Task.CompletedTask

        member this.StopAsync _ =
            do logger.LogInformation("Stopping file event publisher")
            subscriptions |> Seq.iter (fun d -> d.Dispose())
            Task.CompletedTask
