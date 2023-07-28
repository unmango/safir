module Safir.Service.Files

open System
open EventStore.Client
open FSharp.Control
open FSharp.UMX

type FileId = Guid<fileId>
and [<Measure>] fileId

module FileId =
    let inline ofGuid (g: Guid) : FileId = %g
    let inline gen () = Guid.NewGuid() |> ofGuid
    let inline parse (s: string) = Guid.Parse s |> ofGuid
    let inline toGuid (id: FileId) : Guid = %id
    let inline toString (id: FileId) : string = (toGuid id).ToString("N")

[<Literal>]
let Category = "File"

let streamId = StreamId.gen FileId.toString

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
        match event with
        | Events.Discovered file -> Managed { Name = file.Name; Path = file.Path }

    let initial = Initial

module Decisions =
    let discover file state =
        match state with
        | Fold.Initial -> [ Events.Discovered file ]
        | Fold.Managed _ -> []

module View =
    type File = { Id: string; Name: string; Path: string }
    let ofFile id (file: Fold.File) : File = { Id = id; Name = file.Name; Path = file.Path }

    let render id =
        function
        | Fold.Managed file -> ofFile (StreamId.toString id) file
        | _ -> failwith "Unsupported state"

type View = { Files: View.File list }

type Service(client: EventStoreClient) =
    member _.Discover(name, path, ct) =
        let eventData codec event =
            Esdb.toEventData codec (Guid.NewGuid()) (event.GetType().Name) event

        let fileId = FileId.gen ()
        let stream = $"{Category}-{FileId.toString fileId}"

        let file: Events.File = { Name = name; Path = path }

        Esdb.transact client Events.codec eventData Fold.evolve Fold.initial stream (Decisions.discover file) ct
        |> Task.map (View.render (streamId fileId))

    member _.Get(fileId, ct) =
        let stream = $"{Category}-{fileId |> FileId.toString}"
        Esdb.query client Events.codec Fold.evolve Fold.initial stream (streamId fileId |> View.render) ct

    member _.List(ct) =
        Esdb.listCategory client Events.codec Category Fold.evolve Fold.initial View.render ct
