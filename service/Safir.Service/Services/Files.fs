module Safir.Service.Services.Files

open FsCodec.SystemTextJson
open Safir.Service
open TypeShape.UnionContract

module FileId =
    let noop = ()

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

type Service() =
    class
    end
