module Safir.IO.FSharp.DirectoryActionResult

let map f = DirectoryAction.map (Result.map f)

// let retn x = DirectoryAction.retn (Result. x)

// let apply fActionResult xActionResult =
//     let newAction directory =
//         let fResult = DirectoryAction.run directory fActionResult
//         let xResult = DirectoryAction.run directory xActionResult
//         Result.

// let bind f xActionResult =
//     let newAction directory =
//         let xResult =
//             DirectoryAction.run directory xActionResult
//
//         let yAction =
//             match xResult with
//             | Ok x -> f x
//             | Error e -> (Error e) |> DirectoryAction.retn
//
//         DirectoryAction.run directory yAction
//
//     DirectoryAction newAction
