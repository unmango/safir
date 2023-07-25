module Safir.Service.Config

module Codec =
    open System
    open System.Text.Json
    open System.Text.Json.Serialization

    let private options =
        JsonFSharpOptions
            .Default()
            .ToJsonSerializerOptions()

    let serialize<'a> value =
        JsonSerializer.SerializeToUtf8Bytes<'a>(value, options) |> ReadOnlyMemory

    let deserialize<'a> (data: ReadOnlyMemory<byte>) =
        JsonSerializer.Deserialize<'a>(data.Span, options)

    let create<'a> () = Codec.create serialize<'a> deserialize<'a> "application/json"

module Store =
    open EventStore.Client

    let connect connectionString =
        let settings = EventStoreClientSettings.Create(connectionString)
        new EventStoreClient(settings)
