module Safir.Service.Domain.FileSystem

open Equinox
open FSharp.UMX
open FsCodec
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
    let (|Parse|) = parse

let id = FileSystemId.parse "7005bb2b92e24e1eb14121d7c0d5f5a4"

[<Literal>]
let Category = "FileSystem"

let streamId = StreamId.gen FileSystemId.toString

[<return: Struct>]
let (|StreamName|_|) =
    function
    | StreamName.CategoryAndId(Category, FileSystemId.Parse clientId) -> ValueSome clientId
    | _ -> ValueNone

module Events =
    open FsCodec.SystemTextJson

    type File = { Id: FileId }
    type SnapshotData = { Files: FileId array }

    type Event =
        | Added of File
        | Removed of File
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
        | Added file -> { Files = file.Id :: state.Files }
        | Removed file -> { Files = state.Files |> List.filter ((<>) file.Id) }
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

    let add id state =
        match state.Files with
        | Contains id -> false, []
        | _ -> true, [ Events.Added { Id = id } ]

    let remove id state =
        match state.Files with
        | Contains id -> true, [ Events.Removed { Id = id } ]
        | _ -> false, []

type Service internal (resolve: FileSystemId -> Decider<Events.Event, Fold.State>) =
    member _.Add(id, file) =
        let decider = resolve id
        decider.Transact(Decisions.add file)

    member _.Remove(id, file) =
        let decider = resolve id
        decider.Transact(Decisions.remove file)

    member _.Query(id) =
        let decider = resolve id
        decider.Query(fun x -> x.Files)

module Service =
    let create resolve = Service(streamId >> resolve Category)

let private accessStrategy = Config.accessStrategy Fold.isOrigin Fold.toSnapshot

let category store =
    Config.category Events.codec Fold.fold Fold.initial accessStrategy store

let create resolve = category >> resolve >> Service.create
