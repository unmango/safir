namespace Safir.Agent.Configuration

module ConfigurationTypes =

    type AgentOptions() =
        member val DataDirectory: string = null with get, set
        member val EnableGrpcReflection = false with get, set
        member val EnableSwagger = false with get, set
        member val MaxDepth = 0 with get, set

    type DataDirectory = string

    type MaxDepth = int

module DataDirectory =
    open System
    open System.IO
    open ConfigurationTypes

    let parse' exists (value: string) =
        match value with
        | x when String.IsNullOrWhiteSpace(x) -> Error "No data directory configured"
        | x when exists x -> Ok(DataDirectory x)
        | _ -> Error "Data directory does not exist"

    let parse = parse' Directory.Exists
