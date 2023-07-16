module Safir.Service.Domain.FileSystem

open Equinox
open FSharp.UMX
open Safir.Service
open Safir.Service.Domain.Files
open System
open TypeShape.UnionContract

type FileSystemId = Guid<fileId>
and [<Measure>] fileId

module FileSystemId =
    let inline ofGuid (g: Guid) : FileSystemId = %g
    let inline gen () = Guid.NewGuid() |> ofGuid
    let inline parse (s: string) = Guid.Parse s |> ofGuid
    let inline toGuid (id: FileSystemId) : Guid = %id
    let inline toString (id: FileSystemId) : string = (toGuid id).ToString("N")

[<Literal>]
let Category = "FileSystem"

let streamId = StreamId.gen FileSystemId.toString

module Events =
    open FsCodec.SystemTextJson

    type SnapshotData = { Files: FileId array }

    type Event =
        | Added of FileId
        | Removed of FileId
        | Snapshot of SnapshotData

        interface IUnionContract

    let codec = Codec.Create<Event>()

module Reactions =
    let categoryFilter = (=) Files.Category

module Fold =
    open Events

    type State = { Files: FileId list }

    let initial = { Files = [] }

    let evolve state event =
        match event with
        | Added fileId -> { Files = fileId :: state.Files }
        | Removed fileId -> { Files = state.Files |> List.filter ((<>) fileId) }
        | Snapshot snapshot -> { Files = snapshot.Files |> List.ofArray }

    let fold: State -> Events.Event seq -> State = Seq.fold evolve

    let isOrigin =
        function
        | Added _
        | Snapshot _ -> true
        | _ -> false

    let toSnapshot state =
        Snapshot { Files = state.Files |> Array.ofList }

module Decisions =
    open Fold

    let (|Contains|_|) x xs =
        if List.contains x xs then Some Contains else None

    let add file state =
        match state.Files with
        | Contains file -> [ Events.Added file ]
        | _ -> []

    let remove file state =
        match state.Files with
        | Contains file -> []
        | _ -> [ Events.Removed file ]

type Service internal (resolve: FileSystemId -> Decider<Events.Event, Fold.State>) =
    member _.Add(id, file) =
        let decider = resolve id
        decider.Transact(Decisions.add file)

    member _.Remove(id, file) =
        let decider = resolve id
        decider.Transact(Decisions.remove file)

module Service =
    let create resolve = Service(streamId >> resolve Category)

let private accessStrategy = Config.accessStrategy Fold.isOrigin Fold.toSnapshot

let category store =
    Config.category Events.codec Fold.fold Fold.initial accessStrategy store

let create resolve = category >> resolve >> Service.create
