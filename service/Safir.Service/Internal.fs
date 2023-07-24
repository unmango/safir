[<AutoOpen>]
module Safir.Service.Internal

open EventStore.Client

module Esdb =
    let toEventData serialize event =
        EventData(Uuid.NewUuid(), event.GetType().Name, serialize event)
