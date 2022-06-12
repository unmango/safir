namespace Safir.Agent.Configuration

module ConfigurationTypes =

    type AgentOptions() =
        member val DataDirectory: string option = None with get, set
        member val EnableGrpcReflection = false with get, set
        member val EnableSwagger = false with get, set
        member val MaxDepth = 0 with get, set

    type DataDirectory = string

module DataDirectory =
    open System.IO
    open ConfigurationTypes

    let parse' exists (value: string option) =
        match value with
        | Some x when exists x -> Some(DataDirectory x)
        | _ -> None

    let parse = parse' Directory.Exists
