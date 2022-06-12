module Safir.Common

let curry f a b = f (a, b)

let uncurry f (a, b) = f a b
