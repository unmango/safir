module Safir.Service.Config

open System
open Equinox
open Equinox.Core
open Equinox.EventStoreDb
open Serilog

let log = Log.ForContext("isMetric", true)
let resolveDecider category = Decider.resolve log category

type ConnectionStrings = { EventStore: string }

type Options = {
    ConnectionStrings: ConnectionStrings
    CacheMb: int
    MediaDirectory: string
}

[<RequireQualifiedAccess>]
type Store = EventStoreContext * ICache

let createCategory codec initial fold (isOrigin, toSnapshot) (context, cache) =
    let cacheStrategy = CachingStrategy.SlidingWindow(cache, TimeSpan.FromMinutes 20)
    let accessStrategy = AccessStrategy.RollingSnapshots(isOrigin, toSnapshot)
    EventStoreCategory(context, codec, fold, initial, cacheStrategy, accessStrategy)
