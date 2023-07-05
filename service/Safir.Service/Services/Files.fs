module Safir.Service.Services.Files

open Equinox
open FsCodec.SystemTextJson
open FSharp.UMX
open Safir.Service
open TypeShape.UnionContract

type FileId = string<fileId>
and [<Measure>] fileId

module FileId =
    let inline ofString (f: string) : FileId = %f
    let inline toString (id: FileId) : string = %id

[<Literal>]
let Category = "Files"

let streamId = StreamId.gen FileId.toString

module Events =
    type File = { Sha256: string; FullPath: string; Name: string }

    type Event =
        | Discovered of File
        | Tracked

        interface IUnionContract

    let codec = Codec.Create<Event>()

module Fold =
    open Events

    type State =
        | Initial
        | Discovered of File
        | Tracked of File

    let initial = Initial

    let evolve state event =
        match state with
        | Initial ->
            match event with
            | Events.Discovered file -> Discovered file
            | e -> failwithf $"Unexpected %A{e}"
        | Discovered file ->
            match event with
            | Events.Discovered newFile -> Discovered newFile
            | Events.Tracked -> Tracked file
        | Tracked _ -> state

    let fold: State -> Events.Event seq -> State = Seq.fold evolve

module Decisions =
    let discover file state =
        match state with
        | Fold.Initial -> [ Events.Discovered file ]
        | Fold.Discovered existing when existing = file -> []
        | _ -> failwith "Can't discover an existing file"

    let track state =
        match state with
        | Fold.Discovered _ -> [ Events.Tracked ]
        | Fold.Tracked _ -> []
        | _ -> failwith "Can't track an unknown file"

type Service internal (resolve: FileId -> Equinox.Decider<Events.Event, Fold.State>) =
    member _.Discover(id, file) =
        let decider = resolve(id)
        decider.Transact(Decisions.discover file)

    member _.Track(id) =
        let decider = resolve(id)
        decider.Transact(Decisions.track)

let create resolve = Service(streamId >> resolve Category)
