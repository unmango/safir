namespace Safir.Service

open FSharp.UMX
open System

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
