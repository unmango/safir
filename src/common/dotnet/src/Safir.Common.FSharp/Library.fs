module Safir.FSharp.Common

let curry f a b = f (a, b)

let uncurry f (a, b) = f a b

let tap f x =
    f x
    x

let onError f =
    function
    | Error e ->
        f e
        Error e
    | x -> x

type ResultBuilder() =
    member this.Bind(v, f) = Result.bind f v
    member this.Return v = Ok v

let result = ResultBuilder()
