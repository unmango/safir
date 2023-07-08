module Safir.Service.Config

open System
open Equinox
open Equinox.EventStoreDb
open Propulsion.EventStoreDb

let checkpointInterval = TimeSpan.FromHours 1

let accessStrategy isOrigin toSnapshot =
    AccessStrategy.RollingSnapshots(isOrigin, toSnapshot)

let category codec fold initial accessStrategy (context, cache) : Category<'a, 'b, 'c> =
    let cacheStrategy = CachingStrategy.SlidingWindow(cache, TimeSpan.FromMinutes 20)
    EventStoreCategory(context, codec, fold, initial, cacheStrategy, accessStrategy)

module Store =
    let private connector = EventStoreConnector(reqTimeout = TimeSpan.FromSeconds 3, reqRetries = 1)

    let connect name connectionString =
        let strategy = ConnectionStrategy.ClusterTwinPreferSlaveReads
        let discovery = Discovery.ConnectionString connectionString
        connector.Establish(name, discovery, strategy)

    let create name connection =
        EventStoreContext(connection, batchSize = 500), Cache(name, sizeMb = 50)

module Source =
    let create log (connection: EventStoreConnection) checkpoints sink filter =
        let batchSize = 50
        let statsInterval = TimeSpan.FromMinutes 5
        let tailSleepInterval = TimeSpan.FromMinutes 5

        EventStoreSource(
            log,
            statsInterval,
            connection.ReadConnection,
            batchSize,
            tailSleepInterval,
            checkpoints,
            sink,
            categoryFilter = filter
        )
