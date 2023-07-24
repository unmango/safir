module Safir.Service.Ingester

open System
open System.Text
open FsCodec
open Propulsion.Sinks
open Safir.Service.Domain
open Serilog

type Outcome =
    | Ok of used: int * unused: int
    | Skipped of count: int
    | NotApplicable of count: int

let statsInterval = TimeSpan.FromMinutes 1
let stateInterval = TimeSpan.FromMinutes 5

type Stats(logger) =
    inherit Propulsion.Streams.Stats<Outcome>(logger, statsInterval, stateInterval)

    override this.HandleOk(outcome) = () // TODO
    override this.HandleExn(logger, ex) = logger.Information(ex, "Unhandled")

let private renderBody (x: Propulsion.Sinks.EventBody) = Encoding.UTF8.GetString(x.Span)

let private tryDecode<'E> (codec: Propulsion.Sinks.Codec<'E>) (streamName: StreamName) event =
    match codec.TryDecode event with
    | ValueNone when Log.IsEnabled Serilog.Events.LogEventLevel.Debug ->
        Log
            .ForContext("eventData", renderBody event.Data)
            .Debug(
                "Codec {type} Could not decode {eventType} in {stream}",
                codec.GetType().FullName,
                event.EventType,
                streamName
            )

        ValueNone
    | x -> x

let (|Decode|) codec struct (stream, events: Propulsion.Sinks.Event[]) : 'E[] =
    events |> Array.chooseV (tryDecode codec stream)

[<return: Struct>]
let (|Parse|_|) =
    function
    | struct (Files.StreamName clientId, _) & Decode Files.Events.codec events -> ValueSome struct (clientId, events)
    | _ -> ValueNone

let handle (fileSystem: FileSystem.Service) stream events =
    match struct (stream, events) with
    | Parse(fileId, events) ->
        events
        |> Array.map (function
            | Files.Events.Discovered _ -> fileSystem.Add(FileSystem.id, fileId)
            | _ -> async { return false })
        |> Async.Sequential
        |> (fun operation -> async {
            let! results = operation
            let processed = results |> Array.filter id |> Array.length

            return
                if processed = events.Length then
                    StreamResult.AllProcessed, Outcome.Ok(processed, 0)
                else
                    StreamResult.AllProcessed, Outcome.Ok(processed, events.Length - processed)
        })
    | _ -> async { return StreamResult.AllProcessed, Outcome.NotApplicable events.Length }
