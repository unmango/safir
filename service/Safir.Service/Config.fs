module Safir.Service.Config

open System
open Equinox
open Equinox.EventStoreDb

let accessStrategy isOrigin toSnapshot =
    AccessStrategy.RollingSnapshots(isOrigin, toSnapshot)

let category codec fold initial accessStrategy (context, cache) : Category<'a, 'b, 'c> =
    let cacheStrategy = CachingStrategy.SlidingWindow(cache, TimeSpan.FromMinutes 20)
    EventStoreCategory(context, codec, fold, initial, cacheStrategy, accessStrategy)

let connector = EventStoreConnector(reqTimeout = TimeSpan.FromSeconds(3), reqRetries = 1)

let connect connectionString =
    let name = "Test"
    let strategy = ConnectionStrategy.ClusterTwinPreferSlaveReads
    let discovery = Discovery.ConnectionString connectionString
    let connection = connector.Establish(name, discovery, strategy)
    EventStoreContext(connection, batchSize = 500), Cache(name, sizeMb = 50)
