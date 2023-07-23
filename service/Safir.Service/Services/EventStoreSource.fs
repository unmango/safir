namespace Safir.Service.Services

open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Propulsion
open Propulsion.Feed
open Safir.Service
open Safir.Service.Domain
open Serilog

type EventStoreSource
    (logger: ILogger, checkpoints: IFeedCheckpointStore, service: FileSystem.Service, configuration: IConfiguration) =
    inherit BackgroundService()

    override this.ExecuteAsync(stoppingToken) = task {
        let connectionString = configuration.GetConnectionString("EventStore")
        let connection = Config.Store.connect "Ingester" connectionString

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

        let source = Config.Source.create logger connection checkpoints sink FileSystem.Reactions.categoryFilter

        let pipeline = source.Start()
        do! pipeline.AwaitWithStopOnCancellation()
    }
