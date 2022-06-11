namespace Safir.Agent.Services

open System.Threading.Tasks
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Safir.Agent.Configuration
open Safir.Agent.Protos
open Safir.Agent.Queries.ListFiles

type FileSystemService(options: IOptions<AgentOptions>, logger: ILogger<FileSystemService>, ?strategy) =
    inherit FileSystem.FileSystemBase()
    let strategy = defaultArg strategy listFiles

    override this.ListFiles(_, responseStream, context) =
        task {
            let! files = strategy options logger

            logger.LogTrace "Writing to response stream"

            return
                files.Files
                |> Seq.map (fun f -> (f, context.CancellationToken))
                |> Seq.map responseStream.WriteAsync
                |> Task.WhenAll
        }
