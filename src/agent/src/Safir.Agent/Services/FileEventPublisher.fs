namespace Safir.Agent.Services

open System
open System.Threading.Tasks
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Safir.Agent
open Safir.Agent.Services
open System.Reactive.Linq
open Safir.Messaging

type FileEventPublisher(watcher: IFileWatcher, bus: IEventBus, logger: ILogger<FileEventPublisher>) =
    let mutable subscriptions: seq<IDisposable> =
        Seq.empty

    let tryPublish e cancellationToken =
        task {
            try
                logger.LogTrace("Publishing event to bus")
                do! bus.PublishAsync(e, cancellationToken)
            with
            | e -> logger.LogError(e, "Unable to publish event to bus")
        }

    interface IHostedService with
        member this.StartAsync _ =
            do logger.LogTrace("Starting file event publisher")

            subscriptions <-
                [ watcher.Changed
                  |> Observable.map (fun e -> (FileEvent.mapChanged e) :> IEvent)
                  watcher.Created
                  |> Observable.map (fun e -> (FileEvent.mapCreated e) :> IEvent)
                  watcher.Deleted
                  |> Observable.map (fun e -> (FileEvent.mapDeleted e) :> IEvent)
                  watcher.Renamed
                  |> Observable.map (fun e -> (FileEvent.mapRenamed e) :> IEvent) ]
                |> Seq.map (fun o -> o.SelectMany(tryPublish))
                |> Seq.map (fun o -> o.Subscribe())
                |> Seq.toList

            Task.CompletedTask

        member this.StopAsync _ =
            do logger.LogTrace("Stopping file event publisher")
            do subscriptions |> Seq.iter (fun d -> d.Dispose())
            Task.CompletedTask
