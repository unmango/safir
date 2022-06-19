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
