module Safir.Service.Files

open System
open Equinox
open FSharp.Control
open FsCodec.SystemTextJson
open FSharp.UMX
open Safir.V1alpha1
open TypeShape

type FileId = Guid<fileId>
and [<Measure>] fileId

module FileId =
    let inline ofGuid (g: Guid) : FileId = %g
    let inline parse (s: string) = Guid.Parse s |> ofGuid
    let inline toGuid (id: FileId) : Guid = %id
    let inline toString (id: FileId) = (toGuid id).ToString("N")

[<Literal>]
let Category = "File"

let streamId = StreamId.gen FileId.toString

module Events =
    type Event =
        | Created of File
        | Changed of File
        | Deleted of File
        | Renamed of RenamedFile
        | Error of FileWatcherError
        | Discovered of File
        | Snapshot of Snapshot

        interface UnionContract.IUnionContract

    let codec = Codec.Create<Event>()

module FileEvent =
    let changed file = {
        FileEvent.empty () with
            NewHash = "TODO"
            Event = FileEvent.Types.Changed file |> ValueSome
    }

    let created file = {
        FileEvent.empty () with
            NewHash = "TODO"
            Event = FileEvent.Types.Created file |> ValueSome
    }

    let deleted file = {
        FileEvent.empty () with
            NewHash = "TODO"
            Event = FileEvent.Types.Deleted file |> ValueSome
    }

    let error error = {
        FileEvent.empty () with
            NewHash = "TODO"
            Event = FileEvent.Types.Error error |> ValueSome
    }

    let renamed file = {
        FileEvent.empty () with
            NewHash = "TODO"
            Event = FileEvent.Types.Renamed file |> ValueSome
    }

    let discovered file = {
        FileEvent.empty () with
            NewHash = "TODO"
            Event = FileEvent.Types.Discovered file |> ValueSome
    }

module Fold =
    type FileState = { FullPath: string; Hash: string; History: FileEvent list }

    type State =
        | Initial
        | File of FileState

    module FileState =
        let withEvent event (state: FileState) =
            File { state with History = event :: state.History }

        let next path hash event history =
            File { FullPath = path; Hash = hash; History = event :: history }

        let nextFile (file: File) toEvent history =
            next file.FullPath "TODO" (toEvent file) history

        let nextRenamed (file: RenamedFile) history =
            next file.FullPath "TODO" (FileEvent.renamed file) history

    let initial = Initial

    let evolve (state: State) event : State =
        match state, event with
        | _, Events.Snapshot snap ->
            {
                FullPath = snap.FullPath
                Hash = snap.Hash
                History = List.ofSeq snap.History
            }
            |> File
        | Initial, Events.Error _ -> state
        | Initial, Events.Changed file -> FileState.nextFile file FileEvent.changed []
        | Initial, Events.Created file -> FileState.nextFile file FileEvent.created []
        | Initial, Events.Deleted file -> FileState.nextFile file FileEvent.deleted []
        | Initial, Events.Renamed file -> FileState.nextRenamed file []
        | Initial, Events.Discovered file -> FileState.nextFile file FileEvent.discovered []
        | File fileState, Events.Changed file -> FileState.nextFile file FileEvent.changed fileState.History
        | File fileState, Events.Created file -> FileState.nextFile file FileEvent.created fileState.History
        | File fileState, Events.Deleted file -> FileState.nextFile file FileEvent.deleted fileState.History
        | File fileState, Events.Renamed file -> FileState.nextRenamed file fileState.History
        | File fileState, Events.Discovered file -> FileState.nextFile file FileEvent.discovered fileState.History
        | File fileState, Events.Error error -> fileState |> (FileState.withEvent << FileEvent.error) error

    let fold state = Seq.fold evolve state

    let isOrigin =
        function
        | Events.Snapshot _ -> true
        | _ -> false

    let toSnapshot state =
        match state with
        | Initial -> failwith "Unable to snapshot initial state"
        | File fileState ->
            let snap = {
                Snapshot.empty () with
                    FullPath = fileState.FullPath
                    Hash = fileState.Hash
            }

            snap.History.AddRange(fileState.History)
            Events.Snapshot snap

module Decisions =
    let private ifInitial events state =
        if state = Fold.initial then events else []

    let private ifNotInitial events state =
        if state <> Fold.initial then events else []

    let create file = ifInitial [ Events.Created file ]

    let changed file = ifNotInitial [ Events.Changed file ]

    let deleted file = ifNotInitial [ Events.Deleted file ]

    let renamed file = ifNotInitial [ Events.Renamed file ]

    let error (ex: Exception) (_: Fold.State) = [
        Events.Error { FileWatcherError.empty () with Message = ex.Message }
    ]

    let discovered file = ifInitial [ Events.Discovered file ]

type Service internal (resolve: FileId -> Equinox.Decider<Events.Event, Fold.State>) =
    member _.Created(id, file) =
        let decider = resolve id
        decider.Transact(Decisions.create file)

    member _.Changed(id, file) =
        let decider = resolve id
        decider.Transact(Decisions.changed file)

    member _.Deleted(id, file) =
        let decider = resolve id
        decider.Transact(Decisions.deleted file)

    member _.Discovered(id, file) =
        let decider = resolve id
        decider.Transact(Decisions.discovered file)

    member _.Renamed(id, file) =
        let decider = resolve id
        decider.Transact(Decisions.renamed file)

    member _.Error(id, ex) =
        let decider = resolve id
        decider.Transact(Decisions.error ex)

module Service =
    let private resolveCategory store =
        Config.createCategory Events.codec Fold.initial Fold.fold (Fold.isOrigin, Fold.toSnapshot) store

    let create store =
        Service(fun id -> Config.resolveDecider (resolveCategory store) Category (streamId id))
