namespace Safir.Service.Services

open System
open System.IO
open FSharp.Control
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Safir.Service
open Safir.V1alpha1

type DataWatcher(config: IConfiguration, fileSystem: Files.Service, logger: ILogger<DataWatcher>) =
    let mutable _watcher: FileSystemWatcher option = None
    let mutable _subscription: IAsyncRxDisposable = AsyncDisposable.Empty

    let mediaDir = config["MediaDirectory"]

    let onChanged (event: FileSystemEventArgs) =
        logger.LogTrace("Changed: {FullPath}", event.FullPath)

        fileSystem.Changed(
            mediaDir,
            {
                File.empty () with
                    FullPath = event.FullPath
                    Name = event.Name
            }
        )

    let onCreated (event: FileSystemEventArgs) =
        logger.LogTrace("Created: {FullPath}", event.FullPath)
        fileSystem.Created(mediaDir, event.FullPath, event.Name)

    let onDeleted (event: FileSystemEventArgs) =
        logger.LogTrace("Deleted: {FullPath}", event.FullPath)
        fileSystem.Deleted(mediaDir, event.FullPath, event.Name)

    let onError (event: ErrorEventArgs) =
        logger.LogTrace(event.GetException(), "Error")
        fileSystem.Error(mediaDir, event.GetException())

    let onRenamed (event: RenamedEventArgs) =
        logger.LogTrace("Renamed: {OldFullPath} to {FullPath}", event.OldFullPath, event.FullPath)
        fileSystem.Renamed(mediaDir, event.FullPath, event.Name, event.OldFullPath, event.OldName)

    let subscribe (on: 'T -> Async<unit>) (observable: IObservable<'T>) =
        observable
            .ToAsyncObservable()
            .SubscribeAsync(fun n -> async {
                logger.LogTrace("Got notification {Notification}", n)

                match n with
                | OnNext e -> do! on e
                | _ -> ()
            })

    interface IHostedService with
        member this.StartAsync(cancellationToken) = task {
            let mediaDir = config["MediaDirectory"]
            let watcher = new FileSystemWatcher(mediaDir)

            watcher.Changed.Subscribe(fun (e: FileSystemEventArgs) -> logger.LogInformation(e.FullPath))
            |> ignore

            watcher.Created.Subscribe(fun (e: FileSystemEventArgs) -> logger.LogInformation(e.FullPath))
            |> ignore

            watcher.Deleted.Subscribe(fun (e: FileSystemEventArgs) -> logger.LogInformation(e.FullPath))
            |> ignore

            // TODO: Doesn't work
            logger.LogTrace("Subscribing FileSystemWatcher events")

            let! subscriptions =
                [
                    watcher.Changed |> subscribe onChanged
                    watcher.Created |> subscribe onCreated
                    watcher.Deleted |> subscribe onDeleted
                    watcher.Error |> subscribe onError
                    watcher.Renamed |> subscribe onRenamed
                ]
                |> AsyncSeq.ofSeqAsync
                |> AsyncSeq.toListAsync

            logger.LogTrace("Creating a composite subscription")
            _subscription <- AsyncDisposable.Composite subscriptions
            _watcher <- Some watcher
        }

        member this.StopAsync(cancellationToken) = task {
            logger.LogTrace("Disposing FileSystemWatcher subscriptions")
            do! _subscription.DisposeAsync()

            match _watcher with
            | Some w -> w.Dispose()
            | _ -> ()
        }
