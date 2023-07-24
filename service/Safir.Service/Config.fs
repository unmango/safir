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

    let deserialize<'a> (bytes: ReadOnlySpan<byte>) =
        JsonSerializer.Deserialize<'a>(bytes, options)

module Store =
    open EventStore.Client

    let connect connectionString =
        let settings = EventStoreClientSettings.Create(connectionString)
        new EventStoreClient(settings)
