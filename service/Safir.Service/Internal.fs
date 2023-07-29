[<AutoOpen>]
module Safir.Service.Internal

module List =
    let createOrAdd x =
        function
        | Some xs -> x :: xs
        | None -> [ x ]
