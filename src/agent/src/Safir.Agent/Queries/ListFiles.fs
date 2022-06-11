module Safir.Agent.Queries.ListFiles

open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Safir.Agent.Configuration
open Safir.Agent.Protos

type Response =
    { Files: seq<FileSystemEntry> }

let public listFiles (options: IOptions<AgentOptions>) (logger: ILogger) =
    async {
        return { Files = [] }
    }
