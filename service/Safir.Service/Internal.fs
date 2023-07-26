[<AutoOpen>]
module Safir.Service.Internal

open EventStore.Client
open FSharp.Control
open FSharp.UMX
open System

module List =
    let createOrAdd x =
        function
        | Some xs -> x :: xs
        | None -> [ x ]

type Codec<'TEvent>(serialize, deserialize, contentType) =
    member _.ContentType: string = contentType
    member _.Encode(value: 'TEvent) : ReadOnlyMemory<byte> = serialize value
    member _.TryDecode(data: ReadOnlyMemory<byte>) : 'TEvent = deserialize data

module Codec =
    let create<'a> serialize deserialize contentType =
        Codec<'a>(serialize, deserialize, contentType)

type StreamId = string<streamId>
and [<Measure>] streamId

module StreamId =
    let ofRaw: string -> StreamId = UMX.tag

    let gen toString = toString >> ofRaw

    let toString: StreamId -> string = UMX.untag

module StreamName =
    let tryParse (stream: string) =
        match stream.Split('-') with
        | [| category; id |] -> Some(category, StreamId.ofRaw id)
        | _ -> None

    let (|Parse|_|) = tryParse

    let parse = tryParse >> Option.get

    let category = fst << parse

    let streamId = snd << parse

module Esdb =
    let toEventData (codec: Codec<_>) eventId name event =
        EventData(Uuid.FromGuid(eventId), name, codec.Encode(event), contentType = codec.ContentType)

    let loadForward (client: EventStoreClient) stream ct = taskSeq {
        let result =
            client.ReadStreamAsync(Direction.Forwards, stream, StreamPosition.Start, cancellationToken = ct)

        match! result.ReadState with
        | ReadState.Ok -> yield! result :> taskSeq<ResolvedEvent>
        | ReadState.StreamNotFound -> yield! TaskSeq.empty
        | _ -> failwith "Unexpected ReadState result"
    }

    let write (client: EventStoreClient) stream events ct =
        client.AppendToStreamAsync(stream, StreamState.Any, events, cancellationToken = ct)

    let tryDecode (codec: Codec<'a>) (event: ResolvedEvent) = Some(codec.TryDecode(event.Event.Data))

    let aggregate codec fold initial events =
        events |> TaskSeq.map (tryDecode codec) |> TaskSeq.fold fold initial

    let transact (client: EventStoreClient) (codec: Codec<'a>) eventData fold initial stream interpret ct = task {
        let! state = loadForward client stream ct |> aggregate codec fold initial
        let next = interpret state |> Seq.map (eventData codec)
        return! write client stream next ct |> Task.ignore
    }

    let listCategory (client: EventStoreClient) (codec: Codec<_>) (category: string) fold initial render ct =
        let folder state (streamId, data) =
            let event = codec.TryDecode(data)
            state |> Map.change streamId (List.createOrAdd event >> Some)

        loadForward client $"$ce-{category}" ct
        |> TaskSeq.filter (fun r -> not <| r.Event.EventStreamId.StartsWith('$'))
        |> TaskSeq.map (fun r -> StreamName.streamId r.Event.EventStreamId, r.Event.Data)
        |> TaskSeq.fold folder Map.empty
        |> Task.map ((Map.map (fun id -> List.fold fold initial >> render id)) >> Map.values)

type Decider(client: EventStoreClient) =
    member _.Transact() = ()
