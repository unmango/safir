module Safir.Service.Files

open EventStore.Client
open FSharp.Control

module Events =
    type File = { Name: string; Path: string }

    type Event = Discovered of File

    let serialize = Config.Codec.serialize<Event>
    let deserialize span = Config.Codec.deserialize<Event> span

module Fold =
    type File = { Name: string; Path: string }

    type State =
        | Initial
        | Managed of File

    let evolve state event =
        match state with
        | Initial -> state
        | Managed file -> state

    let initial = Initial

module Decisions =
    let discover file state =
        match state with
        | Fold.Initial -> [ Events.Discovered file ]
        | Fold.Managed _ -> []

type Service(client: EventStoreClient) =
    member _.Discover() = task {
        let result = client.ReadStreamAsync(Direction.Forwards, "stream-name", StreamPosition.Start)

        let! state = result.ReadState

        let! state =
            match state with
            | ReadState.Ok -> result :> taskSeq<ResolvedEvent>
            | ReadState.StreamNotFound -> TaskSeq.empty
            | _ -> TaskSeq.empty
            |> TaskSeq.map (fun e -> Events.deserialize e.Event.Data.Span)
            |> TaskSeq.fold Fold.evolve Fold.initial

        let file: Events.File = { Name = "Test"; Path = "Test" }

        let events = Decisions.discover file state |> List.map (Esdb.toEventData Events.serialize)

        let! _ = client.AppendToStreamAsync("stream-name", StreamState.NoStream, events)

        return List.fold Fold.evolve state events
    }
