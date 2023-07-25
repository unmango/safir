[<AutoOpen>]
module Safir.Service.Internal

open EventStore.Client
open FSharp.Control
open System

type Codec<'TEvent>(serialize, deserialize, contentType) =
    member _.ContentType: string = contentType
    member _.Encode(value: 'TEvent) : ReadOnlyMemory<byte> = serialize value
    member _.TryDecode(data: ReadOnlyMemory<byte>) : 'TEvent = deserialize data

module Codec =
    let create<'a> serialize deserialize contentType =
        Codec<'a>(serialize, deserialize, contentType)

type Esdb() =
    static member EventData(eventId, name, data) = ()

module Esdb =
    let toEventData (codec: Codec<_>) eventId name event =
        EventData(Uuid.FromGuid(eventId), name, codec.Encode(event), contentType = codec.ContentType)

    let loadForward (client: EventStoreClient) stream ct = task {
        let result =
            client.ReadStreamAsync(Direction.Forwards, stream, StreamPosition.Start, cancellationToken = ct)

        match! result.ReadState with
        | ReadState.Ok -> return! TaskSeq.toArrayAsync result
        | ReadState.StreamNotFound -> return [||]
        | _ -> return failwith "Unexpected ReadState result"
    }

    let write (client: EventStoreClient) stream events ct =
        client.AppendToStreamAsync(stream, StreamState.Any, events, cancellationToken = ct)

    let tryDecode (codec: Codec<'a>) (event: ResolvedEvent) : 'a option = Some(codec.TryDecode(event.Event.Data))

    let aggregate codec fold initial events =
        events |> Seq.map (tryDecode codec) |> Seq.fold fold initial

    let transact (client: EventStoreClient) (codec: Codec<'a>) eventData fold initial stream interpret ct = task {
        let! events = loadForward client stream ct
        let state = aggregate codec fold initial events
        let next = interpret state |> Seq.map (eventData codec)
        return! write client stream next ct |> Task.ignore
    }

type Decider(client: EventStoreClient) =
    member _.Transact() = ()
