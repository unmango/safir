namespace Safir.Agent.Services

open System.Threading.Tasks
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Safir.Agent.Configuration
open Safir.Agent.Protos
open Safir.Agent.Queries.ListFiles

type FileSystemService(options: IOptions<AgentOptions>, logger: ILogger<FileSystemService>) =
    inherit FileSystem.FileSystemBase()

    override this.ListFiles(_, responseStream, context) =
        task {
            let! files = listFiles options logger

            return
                files.Files
                |> Seq.map (fun f -> (f, context.CancellationToken))
                |> Seq.map responseStream.WriteAsync
                |> Task.WhenAll
        }
