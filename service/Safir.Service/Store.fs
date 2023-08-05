module Safir.Service.Store

open EventStore.Client
open FSharp.Control

module Esdb =
    let toEventData (codec: Codec<_>) eventId name event =
        EventData(Uuid.FromGuid(eventId), name, codec.Encode(event), contentType = codec.ContentType)

    let loadForward (client: EventStoreClient) stream ct = taskSeq {
        let result =
            client.ReadStreamAsync(
                Direction.Forwards,
                stream,
                StreamPosition.Start,
                resolveLinkTos = true,
                cancellationToken = ct
            )

        match! result.ReadState with
        | ReadState.Ok -> yield! result :> taskSeq<ResolvedEvent>
        | ReadState.StreamNotFound -> yield! TaskSeq.empty
        | _ -> failwith "Unexpected ReadState result"
    }

    let write (client: EventStoreClient) stream events ct =
        client.AppendToStreamAsync(stream, StreamState.Any, events, cancellationToken = ct)

    let tryDecode (codec: Codec<'a>) (event: ResolvedEvent) = Some(codec.TryDecode(event.Event.Data))

    let aggregate codec fold initial events =
        events
        |> TaskSeq.map (tryDecode codec)
        |> TaskSeq.choose id
        |> TaskSeq.fold fold initial

    let transact (client: EventStoreClient) (codec: Codec<'a>) eventData fold initial stream interpret ct = task {
        let! state = loadForward client stream ct |> aggregate codec fold initial
        let events = interpret state
        let next = events |> Seq.map (eventData codec)
        do! write client stream next ct |> Task.ignore
        return Seq.fold fold state events
    }

    let query (client: EventStoreClient) (codec: Codec<'a>) fold initial stream render ct =
        loadForward client stream ct |> aggregate codec fold initial |> Task.map render

    let listCategory (client: EventStoreClient) (codec: Codec<_>) (category: string) fold initial render ct =
        let folder state (streamId, data) =
            let event = codec.TryDecode(data)
            state |> Map.change streamId (List.createOrAdd event >> Some)

        loadForward client $"$ce-{category}" ct
        |> TaskSeq.map (fun r -> StreamName.streamId r.Event.EventStreamId, r.Event.Data)
        |> TaskSeq.fold folder Map.empty
        |> Task.map ((Map.map (fun id -> List.fold fold initial >> render id)) >> Map.values)