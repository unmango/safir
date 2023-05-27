module Safir.Service.FileSystem

open System
open Equinox
open FsCodec.SystemTextJson
open TypeShape

[<Literal>]
let Category = "FileSystem"

let streamId = StreamId.gen id

module Events =
    type File = { FullPath: string; Name: string }

    type RenamedFile = {
        FullPath: string
        Name: string
        OldFullPath: string
        OldName: string
    }

    type Error = { Exception: Exception }
    type Snapshot = { Files: File[]; Errors: Error[] }

    type Event =
        | Created of File
        | Changed of File
        | Deleted of File
        | Renamed of RenamedFile
        | Error of Error
        | Discovered of File
        | Snapshot of Snapshot

        interface UnionContract.IUnionContract

    let codec = Codec.Create<Event>()

module Fold =
    type State = { Files: Events.File list; Errors: Events.Error list }

    let initial: State = { Files = []; Errors = [] }

    let evolve state =
        function
        | Events.Created file -> { state with Files = file :: state.Files }
        | Events.Changed _ -> state
        | Events.Deleted file -> {
            state with
                Files = state.Files |> List.filter ((<>) file)
          }
        | Events.Renamed file -> {
            state with
                Files =
                    {
                        Events.File.FullPath = file.FullPath
                        Events.File.Name = file.Name
                    }
                    :: state.Files
                    |> List.filter (fun f -> f.FullPath <> file.OldFullPath)
          }
        | Events.Error error -> { state with Errors = error :: state.Errors }
        | Events.Discovered file -> { state with Files = file :: state.Files }
        | Events.Snapshot snapshot -> {
            Files = List.ofArray snapshot.Files
            Errors = List.ofArray snapshot.Errors
          }

    let fold state = Seq.fold evolve state

    let isOrigin =
        function
        | Events.Snapshot _ -> true
        | _ -> false

    let toSnapshot state =
        Events.Snapshot {
            Files = Array.ofList state.Files
            Errors = Array.ofList state.Errors
        }

let decideCreate path name (_: Fold.State) = [ Events.Created { FullPath = path; Name = name } ]

let decideChanged path name (_: Fold.State) = [ Events.Changed { FullPath = path; Name = name } ]

let decideDeleted path name (_: Fold.State) = [ Events.Deleted { FullPath = path; Name = name } ]

let decideRenamed path name oldPath oldName (_: Fold.State) = [
    Events.Renamed {
        FullPath = path
        Name = name
        OldFullPath = oldPath
        OldName = oldName
    }
]

let decideError ex (_: Fold.State) = [ Events.Error { Exception = ex } ]

let decideDiscovered path name (state: Fold.State) =
    let proposed: Events.File = { FullPath = path; Name = name }

    if state.Files |> List.contains proposed then
        []
    else
        [ Events.Discovered proposed ]

type Service internal (resolve: string -> Equinox.Decider<Events.Event, Fold.State>) =
    member _.Created(id, path, name) =
        let decider = resolve id
        decider.Transact(decideCreate path name)

    member _.Changed(id, path, name) =
        let decider = resolve id
        decider.Transact(decideChanged path name)

    member _.Deleted(id, path, name) =
        let decider = resolve id
        decider.Transact(decideDeleted path name)

    member _.Discovered(id, path, name) =
        let decider = resolve id
        decider.Transact(decideDiscovered path name)

    member _.Renamed(id, path, name, oldPath, oldName) =
        let decider = resolve id
        decider.Transact(decideRenamed path name oldPath oldName)

    member _.Error(id, ex) =
        let decider = resolve id
        decider.Transact(decideError ex)

module Service =
    let private resolveCategory store =
        Config.createCategory Events.codec Fold.initial Fold.fold (Fold.isOrigin, Fold.toSnapshot) store

    let create store =
        Service(fun id -> Config.resolveDecider (resolveCategory store) Category (streamId id))
