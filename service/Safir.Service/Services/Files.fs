module Safir.Service.Services.Files

open Equinox
open FsCodec.SystemTextJson
open FSharp.UMX
open Safir.Service
open TypeShape.UnionContract

type FileId = string<fullPath> * string<sha256>
and [<Measure>] fullPath
and [<Measure>] sha256

module FileId =
    let inline from (p: string) (s: string) : FileId = %p, %s
    let inline toString ((p, s): FileId) : string = $"{%p}-{%s}"

[<Literal>]
let Category = "Files"

let streamId = StreamId.gen FileId.toString

module Events =
    type File = { Sha256: string; FullPath: string; Name: string }
    type Snapshot = { Tracked: bool; File: File }

    type Event =
        | Discovered of File
        | Tracked
        | Snapshot of Snapshot

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

let accessStrategy = Config.accessStrategy Fold.isOrigin Fold.toSnapshot

let category store =
    Config.category Events.codec Fold.fold Fold.initial accessStrategy store

let create resolve = category >> resolve >> Service.create
