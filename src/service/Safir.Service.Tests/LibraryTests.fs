module Safir.Service.Tests.LibraryTests

open FsCheck
open FsCheck.Xunit
open Safir.Service.Library

[<Property>]
let ``FileDiscovered adds to untracked`` (state: Fold.State) (file: Events.File) =
    let event = Events.FileDiscovered file
    let result = Fold.evolve state event
    file :: state.UnTracked = result.UnTracked

[<Property>]
let ``FileDiscovered doesn't modify tracked`` (state: Fold.State) (file: Events.File) =
    let event = Events.FileDiscovered file
    let result = Fold.evolve state event
    state.Tracked = result.Tracked

[<Property>]
let ``FileDiscovered doesn't modify removed`` (state: Fold.State) (file: Events.File) =
    let event = Events.FileDiscovered file
    let result = Fold.evolve state event
    state.Removed = result.Removed

[<Property>]
let ``FileTracked removes from untracked`` (state: Fold.State) (file: Events.File) =
    let event = Events.FileTracked file
    let result = Fold.evolve state event
    not <| List.contains file result.UnTracked

[<Property>]
let ``FileTracked maintains untracked`` (state: Fold.State) (file: Events.File) =
    let event = Events.FileTracked file
    let result = Fold.evolve state event
    Set.isSubset (Set.ofList result.UnTracked) (Set.ofList state.UnTracked)

[<Property>]
let ``FileTracked adds to tracked`` (state: Fold.State) (file: Events.File) =
    let event = Events.FileTracked file
    let result = Fold.evolve state event
    List.contains file result.Tracked

[<Property>]
let ``FileTracked does not modify removed`` (state: Fold.State) (file: Events.File) =
    let event = Events.FileTracked file
    let result = Fold.evolve state event
    state.Removed = result.Removed

[<Property>]
let ``FileUntracked maintains tracked`` (state: Fold.State) (file: Events.File) =
    let event = Events.FileUntracked file
    let result = Fold.evolve state event
    Set.isSubset (Set.ofList result.Tracked) (Set.ofList state.Tracked)

[<Property>]
let ``FileUntracked removes from tracked`` (state: Fold.State) (file: Events.File) =
    let event = Events.FileUntracked file
    let result = Fold.evolve state event
    not <| List.contains file result.Tracked

[<Property>]
let ``FileUntracked adds to untracked`` (state: Fold.State) (file: Events.File) =
    let event = Events.FileUntracked file
    let result = Fold.evolve state event
    List.contains file result.UnTracked

[<Property>]
let ``FileUntracked does not modify removed`` (state: Fold.State) (file: Events.File) =
    let event = Events.FileUntracked file
    let result = Fold.evolve state event
    state.Removed = result.Removed

[<Property>]
let ``FileRemoved removes from tracked`` (state: Fold.State) (file: Events.File) =
    let event = Events.FileRemoved file
    let result = Fold.evolve state event
    not <| List.contains file result.Tracked

[<Property>]
let ``FileRemoved removes from unTracked`` (state: Fold.State) (file: Events.File) =
    let event = Events.FileRemoved file
    let result = Fold.evolve state event
    not <| List.contains file result.UnTracked

[<Property>]
let ``FileRemoved adds to removed`` (state: Fold.State) (file: Events.File) =
    let event = Events.FileRemoved file
    let result = Fold.evolve state event
    List.contains file result.Removed

[<Property>]
let ``Snapshot initializes removed`` (state: Fold.State) (files: Events.File[]) =
    let event =
        Events.Snapshot {
            Removed = files
            Tracked = Array.empty
            UnTracked = Array.empty
        }

    let result = Fold.evolve state event
    List.ofArray files = result.Removed

[<Property>]
let ``Snapshot initializes tracked`` (state: Fold.State) (files: Events.File[]) =
    let event =
        Events.Snapshot {
            Removed = Array.empty
            Tracked = files
            UnTracked = Array.empty
        }

    let result = Fold.evolve state event
    List.ofArray files = result.Tracked

[<Property>]
let ``Snapshot initializes untracked`` (state: Fold.State) (files: Events.File[]) =
    let event =
        Events.Snapshot {
            Removed = Array.empty
            Tracked = Array.empty
            UnTracked = files
        }

    let result = Fold.evolve state event
    List.ofArray files = result.UnTracked

[<Property>]
let ``decideDiscover discovers new paths`` (state: Fold.State) (path: string) =
    let precondition =
        not (state.UnTracked |> List.contains { Path = path })
        && not (state.Tracked |> List.contains { Path = path })

    let fileDiscovered =
        let events = decideDiscover path state
        events = [ Events.FileDiscovered { Path = path } ]

    precondition ==> fileDiscovered

[<Property>]
let ``decideDiscover ignores existing paths`` (state: Fold.State) (path: string) =
    let precondition =
        state.UnTracked |> List.contains { Path = path }
        || state.Tracked |> List.contains { Path = path }

    let fileDiscovered =
        let events = decideDiscover path state
        events.Length = 0

    precondition ==> fileDiscovered
