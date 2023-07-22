[<AutoOpen>]
module Safir.Service.Internal

module Array =
    let inline chooseV f xs = [|
        for item in xs do
            match f item with
            | ValueSome v -> yield v
            | ValueNone -> ()
    |]
