module Safir.Agent.Queries.ListFiles

open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Safir.Agent.Configuration
open Safir.Agent.Protos

type Response =
    { Files: seq<FileSystemEntry> }

let listFiles (options: IOptions<AgentOptions>) (logger: ILogger) =
    async {
        logger.LogInformation("Executing")
        return { Files = [
            FileSystemEntry(Path = "Test")
            FileSystemEntry(Path = "Test2")
        ] }
    }
