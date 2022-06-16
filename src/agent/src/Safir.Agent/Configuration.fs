namespace Safir.Agent.Configuration

open System.IO.Abstractions

type AgentOptions() =
    member val DataDirectory: string = null with get, set
    member val EnableGrpcReflection = false with get, set
    member val EnableSwagger = false with get, set
    member val MaxDepth = 0 with get, set

type DataDirectory = DataDirectory of string

module DataDirectory =
    open System
    open System.IO
    open Safir.Common.Reader

    let parse' exists (value: string) =
        match value with
        | x when String.IsNullOrWhiteSpace(x) -> Error "No data directory configured"
        | x when exists x -> Ok(DataDirectory x)
        | _ -> Error "Data directory does not exist"

    let parse = parse' Directory.Exists

    let get = reader {
        let! (directory: IDirectory) = ask
        return parse' directory.Exists
    }
