namespace Safir.Agent.Tests.Services

open System.IO
open System.Reactive.Subjects
open System.Threading
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Moq
open Safir.Agent.Protos
open Safir.Agent.Services
open Safir.Messaging
open Xunit

type FileEventPublisherTests() =
    let changed = new Subject<FileSystemEventArgs>()
    let created = new Subject<FileSystemEventArgs>()
    let deleted = new Subject<FileSystemEventArgs>()
    let renamed = new Subject<RenamedEventArgs>()
    let watcher = Mock<IFileWatcher>()
    let bus = Mock<IEventBus>()

    let publisher =
        watcher.SetupGet(fun x -> x.Changed).Returns(changed)
        |> ignore
        watcher.SetupGet(fun x -> x.Created).Returns(created)
        |> ignore
        watcher.SetupGet(fun x -> x.Deleted).Returns(deleted)
        |> ignore
        watcher.SetupGet(fun x -> x.Renamed).Returns(renamed)
        |> ignore

        FileEventPublisher(watcher.Object, bus.Object, Mock.Of<ILogger<FileEventPublisher>>())

    [<Fact>]
    let ``Subscribes changed observable`` () =
        task {
            do! (publisher :> IHostedService).StartAsync(CancellationToken.None)
            changed.OnNext(FileSystemEventArgs(WatcherChangeTypes.Changed, "", ""))

            bus.Verify(fun x -> x.PublishAsync<IEvent>(It.IsAny<FileChanged>(), It.IsAny<CancellationToken>()))
        }

    [<Fact>]
    let ``Subscribes created observable`` () =
        task {
            do! (publisher :> IHostedService).StartAsync(CancellationToken.None)
            created.OnNext(FileSystemEventArgs(WatcherChangeTypes.Created, "", ""))

            bus.Verify(fun x -> x.PublishAsync<IEvent>(It.IsAny<FileCreated>(), It.IsAny<CancellationToken>()))
        }

    [<Fact>]
    let ``Subscribes deleted observable`` () =
        task {
            do! (publisher :> IHostedService).StartAsync(CancellationToken.None)
            deleted.OnNext(FileSystemEventArgs(WatcherChangeTypes.Deleted, "", ""))

            bus.Verify(fun x -> x.PublishAsync<IEvent>(It.IsAny<FileDeleted>(), It.IsAny<CancellationToken>()))
        }

    [<Fact>]
    let ``Subscribes renamed observable`` () =
        task {
            do! (publisher :> IHostedService).StartAsync(CancellationToken.None)
            renamed.OnNext(RenamedEventArgs(WatcherChangeTypes.Renamed, "", "", ""))

            bus.Verify(fun x -> x.PublishAsync<IEvent>(It.IsAny<FileRenamed>(), It.IsAny<CancellationToken>()))
        }
