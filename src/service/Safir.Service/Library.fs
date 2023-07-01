module Safir.Service.Library

open Equinox

[<Literal>]
let Category = "Library"

let streamId = StreamId.gen id

module Events =
    type File = { Path: string }
    type Snapshot = { Removed: File[]; Tracked: File[]; UnTracked: File[] }

    type Event =
        | FileDiscovered of File
        | FileTracked of File
        | FileUntracked of File
        | FileRemoved of File
        | Snapshot of Snapshot

    let codec = Codec.Create<Event>()

module Fold =
    type State = {
        Removed: Events.File list
        Tracked: Events.File list
        UnTracked: Events.File list
    }

    let initial: State = { Tracked = []; UnTracked = []; Removed = [] }

    let evolve state =
        function
        | Events.FileDiscovered file -> { state with UnTracked = file :: state.UnTracked }
        | Events.FileTracked file -> {
            state with
                Tracked = file :: state.Tracked
                UnTracked = state.UnTracked |> List.filter ((<>) file)
          }
        | Events.FileUntracked file -> {
            state with
                Tracked = state.Tracked |> List.filter ((<>) file)
                UnTracked = file :: state.UnTracked
          }
        | Events.FileRemoved file -> {
            Removed = file :: state.Removed
            Tracked = state.Tracked |> List.filter ((<>) file)
            UnTracked = state.UnTracked |> List.filter ((<>) file)
          }
        | Events.Snapshot snapshot -> {
            Removed = List.ofArray snapshot.Removed
            Tracked = List.ofArray snapshot.Tracked
            UnTracked = List.ofArray snapshot.UnTracked
          }

    let fold = Seq.fold evolve

    let isOrigin =
        function
        | Events.Snapshot _ -> true
        | _ -> false

    let toSnapshot state =
        Events.Snapshot {
            Removed = Array.ofList state.Removed
            Tracked = Array.ofList state.Tracked
            UnTracked = Array.ofList state.UnTracked
        }

let decideDiscover path (state: Fold.State) =
    let proposed: Events.File = { Path = path }
    let tracked = state.Tracked |> List.contains proposed
    let unTracked = state.UnTracked |> List.contains proposed

    if not unTracked && not tracked then
        [ Events.FileDiscovered proposed ]
    else
        []

let decideTrack path (state: Fold.State) =
    let proposed: Events.File = { Path = path }
    let tracked = state.Tracked |> List.contains proposed
    let unTracked = state.UnTracked |> List.contains proposed

    if unTracked && not tracked then
        [ Events.FileTracked proposed ]
    else
        []

let decideUntrack path (state: Fold.State) =
    let proposed: Events.File = { Path = path }
    let tracked = state.Tracked |> List.contains proposed
    let unTracked = state.UnTracked |> List.contains proposed

    if tracked && not unTracked then
        [ Events.FileUntracked proposed ]
    else
        []

let decideRemove path (state: Fold.State) =
    let proposed: Events.File = { Path = path }
    let removed = state.Removed |> List.contains proposed
    let tracked = state.Tracked |> List.contains proposed
    let unTracked = state.UnTracked |> List.contains proposed

    if not removed && (tracked || unTracked) then
        [ Events.FileRemoved proposed ]
    else
        []

type ItemView = { Path: string }

type View = {
    Removed: ItemView seq
    Tracked: ItemView seq
    UnTracked: ItemView seq
}

let private renderItem (item: Events.File) : ItemView = { Path = item.Path }

let private render (state: Fold.State) : View = {
    Removed = state.Removed |> Seq.map renderItem
    Tracked = state.Tracked |> Seq.map renderItem
    UnTracked = state.UnTracked |> Seq.map renderItem
}

type Service internal (resolve: string -> Equinox.Decider<Events.Event, Fold.State>) =
    member _.List instanceId : Async<View> =
        let decider = resolve instanceId
        decider.Query(render)

    member _.Execute(instanceId, command) : Async<unit> =
        let decider = resolve instanceId
        decider.Transact command

    member _.Discover(instanceId, path) =
        let decider = resolve instanceId
        decider.Transact(decideDiscover path)

    member _.Track(instanceId, path) =
        let decider = resolve instanceId
        decider.Transact(decideTrack path)

    member _.UnTrack(instanceId, path) =
        let decider = resolve instanceId
        decider.Transact(decideUntrack path)

    member _.Remove(instanceId, path) =
        let decider = resolve instanceId
        decider.Transact(decideRemove path)

module Service =
    let private resolveCategory store =
        Config.createCategory Events.codec Fold.initial Fold.fold (Fold.isOrigin, Fold.toSnapshot) store

    let create store =
        Service(fun id -> Config.resolveDecider (resolveCategory store) Category (streamId id))
