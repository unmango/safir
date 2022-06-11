module Safir.Agent.Configuration

open System.IO

type AgentOptions() =
    member val DataDirectory: string option = None with get, set
    member val EnableGrpcReflection = false with get, set
    member val EnableSwagger = false with get, set
    member val MaxDepth = 0 with get, set

type DataDirectory = string

let getDataDirectory' exists (value: string option) =
    match value with
    | Some x when exists x -> Some (DataDirectory x)
    | _ -> None

let getDataDirectory = getDataDirectory' Directory.Exists
