namespace Safir.Service

open System.Collections.Concurrent
open Propulsion.Feed
open Serilog

type CheckpointStore(logger: ILogger, defaultCheckpointFrequency) =
    let store = ConcurrentDictionary<string, Position>()

    let addValue _ position = position
    let updateValue _ _ position = position

    let streamName source tranche =
        let source = source |> SourceId.toString |> (fun x -> x.TrimStart('$', '_'))
        sprintf "%s-%s_%s" source (TrancheId.toString tranche) "checkpoint"

    interface IFeedCheckpointStore with
        member this.Commit(source, tranche, position, cancellationToken) = task {
            let stream = streamName source tranche
            logger.Debug("Committing position for {Stream}: {Position}", stream, position)
            store.AddOrUpdate(stream, addValue, updateValue, position) |> ignore
        }

        member this.Start(source, tranche, establishOrigin, cancellationToken) = task {
            let stream = streamName source tranche
            let found, position = store.TryGetValue(stream)

            let! position =
                match found, position, establishOrigin with
                | true, position, _ ->
                    logger.Debug("Getting position for stream {Stream}: {Position}", stream, position)
                    task { return position }
                | false, _, Some establishOrigin ->
                    logger.Debug("Establishing position for stream {Stream}", stream, position)
                    establishOrigin.Invoke cancellationToken
                | false, _, None ->
                    logger.Debug("Using initial position for stream {Stream}", stream)
                    task { return Position.initial }

            return struct (defaultCheckpointFrequency, position)
        }
