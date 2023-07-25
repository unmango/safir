module Safir.Service.Files

open System
open System.Text
open EventStore.Client
open FSharp.Control
open FSharp.UMX

type FileId = Guid<fileId>
and [<Measure>] fileId

module FileId =
    let inline ofGuid (g: Guid) : FileId = %g
    let inline parse (s: string) = Guid.Parse s |> ofGuid
    let inline toGuid (id: FileId) : Guid = %id
    let inline toString (id: FileId) : string = (toGuid id).ToString("N")

[<Literal>]
let Category = "File"

module Events =
    type File = { Name: string; Path: string }

    type Event = Discovered of File

    let codec = Config.Codec.create<Event> ()

module Fold =
    type File = { Name: string; Path: string }

    type State =
        | Initial
        | Managed of File

    let evolve state event =
        match state with
        | Initial -> state
        | Managed file -> state

    let initial = Initial

module Decisions =
    let discover file state =
        match state with
        | Fold.Initial -> [ Events.Discovered file ]
        | Fold.Managed _ -> []

module View =
    type File = { Id: string; Name: string; Path: string }
    let ofFile id (file: Events.File) : File = { Id = id; Name = file.Name; Path = file.Path }

type View = { Files: View.File list }

type Service(client: EventStoreClient) =
    member _.Discover(fileId, name, path, ct) =
        let eventData codec event =
            Esdb.toEventData codec (Guid.NewGuid()) (event.GetType().Name) event

        let stream = $"{Category}-{fileId |> FileId.toString}"

        let file: Events.File = { Name = name; Path = path }
        Esdb.transact client Events.codec eventData Fold.evolve Fold.initial stream (Decisions.discover file) ct

    member _.List() =
        client.ReadStreamAsync(Direction.Forwards, "$ce-File", StreamPosition.Start, resolveLinkTos = true)
        |> TaskSeq.map (fun x ->
            let parts = x.Event.EventStreamId.Split('-')
            let category = parts[0]
            let fileId = parts[1]
            let event = Events.codec.TryDecode(x.Event.Data)
            fileId, event)
        |> TaskSeq.fold
            (fun s (i, e) ->
                match e with
                | Events.Discovered f -> { Files = View.ofFile i f :: s.Files })
            { Files = [] }
