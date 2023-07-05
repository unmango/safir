module Safir.Service.Codec

open System
open System.IO
open FsCodec
open Google.Protobuf
open Safir.V1alpha1

let private descriptors =
    lazy
        [ FilesReflection.Descriptor() ]
        |> Seq.collect (fun d -> d.MessageTypes)
        |> Seq.map (fun d -> d.FullName, d)
        |> Map

let private encode<'e when 'e :> IMessage> (e: 'e) : struct (string * ReadOnlyMemory<byte>) =
    let formatter = JsonFormatter.Default
    use stream = new MemoryStream()
    use writer = new StreamWriter(stream)
    formatter.Format(e, writer)
    e.Descriptor.FullName, ReadOnlyMemory<byte>(stream.ToArray())

let private tryDecode<'e when 'e :> IMessage> n (m: ReadOnlyMemory<byte>) : 'e voption =
    let success, descriptor = descriptors.Value.TryGetValue n

    if not success then
        ValueNone
    else
        let parser = JsonParser.Default
        use stream = new MemoryStream(m.ToArray())
        use reader = new StreamReader(stream)

        try
            let message = parser.Parse(reader, descriptor)
            assert (n = message.Descriptor.Name)
            ValueSome(message :?> 'e)
        with _ ->
            ValueNone

let customCodec<'Event when 'Event: (new: unit -> 'Event) and 'Event :> IMessage> () =
    Codec.Create<'Event, ReadOnlyMemory<byte>>(encode, tryDecode)
