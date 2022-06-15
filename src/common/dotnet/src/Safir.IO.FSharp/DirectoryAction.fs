module Safir.IO.FSharp.DirectoryAction

open System.IO.Abstractions
open Safir.IO.FSharp.Directory

let run d (DirectoryAction a) = a d

let map f a = DirectoryAction(fun d -> run d a |> f)

let retn x = DirectoryAction(fun _ -> x)

let apply fAction xAction =
    let newAction directory =
        let f = run directory fAction
        let x = run directory xAction
        f x

    DirectoryAction newAction

let bind f a =
    DirectoryAction(fun d -> run d a |> f |> run d)

let execute action =
    let directory = DirectoryWrapper(FileSystem())
    run directory action
