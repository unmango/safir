namespace Safir.Service

open EventStore.Client
open Safir.Service.Store

type Decider<'TEvent, 'TState>
    (
        client: EventStoreClient,
        codec: Codec<'TEvent>,
        category: string,
        fold: 'TState -> 'TEvent -> 'TState,
        initial: 'TState,
        stream: string
    ) =
    let toEventData = Esdb.toEventData codec
    let loadForward = Esdb.loadForward client stream
    let write = Esdb.write client stream
    let tryDecode = Esdb.tryDecode codec
    let aggregate = Esdb.aggregate codec fold initial
    // let transact = Esdb.transact client codec (fun codec event -> Esdb.toEventData codec)
    let query render = Esdb.query client codec fold initial stream render
    let listCategory = Esdb.listCategory client codec category fold initial

    member _.Query(render, ct) = query render ct

    member _.Transact(interpret, ct) = ()
