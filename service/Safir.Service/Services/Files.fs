module Safir.Service.Services.Files

open Equinox
open FSharp.UMX
open Safir.Service
open System
open TypeShape.UnionContract

type FileId = Guid<fileId>
and [<Measure>] fileId

module FileId =
    let inline ofGuid (g: Guid) : FileId = %g
    let inline gen () = Guid.NewGuid() |> ofGuid
    let inline parse (s: string) = Guid.Parse s |> ofGuid
    let inline toGuid (id: FileId) : Guid = %id
    let inline toString (id: FileId) : string = (toGuid id).ToString("N")

[<Literal>]
let Category = "Files"

let streamId = StreamId.gen FileId.toString

module Events =
    open FsCodec.SystemTextJson

    type File = { Sha256: string; FullPath: string; Name: string }
    type SnapshotData = { Tracked: bool; File: File }

    type Event =
        | Discovered of File
        | Tracked
        | Snapshot of SnapshotData

        interface IUnionContract

    let codec = Codec.Create<Event>()

module Fold =
    open Events

    type ManagedFile = { Tracked: bool; File: File }

    type State =
        | Initial
        | Managed of ManagedFile

    let initial = Initial

    let evolve state event =
        match state with
        | Initial ->
            match event with
            | Discovered file -> { Tracked = false; File = file }
            | Snapshot snapshot -> { Tracked = snapshot.Tracked; File = snapshot.File }
            | e -> failwithf $"Unexpected %A{e}"
        | Managed file ->
            match event with
            | Tracked -> { file with Tracked = true }
            | Snapshot snapshot -> { Tracked = snapshot.Tracked; File = snapshot.File }
            | e -> failwithf $"Unexpected %A{e}"
        |> Managed

    let fold: State -> Events.Event seq -> State = Seq.fold evolve

    let isOrigin =
        function
        | Snapshot _ -> true
        | _ -> false

    let toSnapshot =
        function
        | Initial -> failwith "Can't snapshot initial state"
        | Managed file -> Snapshot { Tracked = file.Tracked; File = file.File }

module Decisions =
    let discover file state =
        match state with
        | Fold.Initial -> [ Events.Discovered file ]
        | Fold.Managed existing when existing.File = file -> []
        | _ -> failwith "Can't discover an existing file"

    let track state =
        match state with
        | Fold.Managed file -> if file.Tracked then [] else [ Events.Tracked ]
        | _ -> failwith "Can't track an unknown file"

type Service internal (resolve: FileId -> Decider<Events.Event, Fold.State>) =
    member _.Discover(id, file) =
        let decider = resolve id
        decider.Transact(Decisions.discover file)

    member _.Track(id) =
        let decider = resolve id
        decider.Transact(Decisions.track)

module Service =
    let create resolve = Service(streamId >> resolve Category)

let private accessStrategy = Config.accessStrategy Fold.isOrigin Fold.toSnapshot

let category store =
    Config.category Events.codec Fold.fold Fold.initial accessStrategy store

let create resolve = category >> resolve >> Service.create
