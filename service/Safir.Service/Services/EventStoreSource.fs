namespace Safir.Service.Services

open Microsoft.Extensions.Hosting
open Propulsion
open Propulsion.Feed
open Safir.Service
open Safir.Service.Domain
open Serilog

type EventStoreSource(logger: ILogger, checkpoints: IFeedCheckpointStore, service: FileSystem.Service) =
    inherit BackgroundService()

    override this.ExecuteAsync(stoppingToken) = task {
        let connection = Config.Store.connect "Test" (failwith "TODO")

        let maxReadAhead = 16
        let maxConcurrentStreams = 8

        let sink =
            Sinks.Factory.StartConcurrent(
                logger,
                maxReadAhead,
                maxConcurrentStreams,
                Ingester.handle service,
                Ingester.Stats(logger)
            )

        let filter _ = true

        let source = Config.Source.create logger connection checkpoints sink filter

        let pipeline = source.Start()
        do! pipeline.AwaitWithStopOnCancellation()
    }
