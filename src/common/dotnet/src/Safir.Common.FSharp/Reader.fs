namespace Safir.Common

type Reader<'environment, 'a> = Reader of ('environment -> 'a)

module Reader =
    let run e (Reader a) = a e

    let map f a = Reader(fun e -> run e a |> f)

    let retn x = Reader(fun _ -> x)

    let apply fAction xAction =
        let newAction environment =
            let f = run environment fAction
            let x = run environment xAction
            f x

        Reader newAction

    let bind f a =
        Reader(fun e -> run e a |> f |> run e)

    let ask = Reader id

    let withEnv (f: 'a -> 'b) r =
        Reader (fun a -> (run (f a) r))

    type ReaderBuilder() =
        member _.Return(x) = Reader (fun _ -> x)
        member _.Bind(x, f) = bind f x
        member _.Zero() = Reader (fun _ -> ())

    let reader = ReaderBuilder()
