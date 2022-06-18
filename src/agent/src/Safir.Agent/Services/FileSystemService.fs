namespace Safir.Agent.Services

open System.Threading.Tasks
open Microsoft.Extensions.Logging
open Safir.Agent.Protos
open Safir.Agent.Queries.ListFiles

type FileSystemService(logger: ILogger<FileSystemService>, strategy) =
    inherit FileSystem.FileSystemBase()

    override this.ListFiles(_, responseStream, context) =
        strategy ()
        |> function
            | Ok x -> x
            | Error e ->
                do logger.LogInformation e
                []
        |> Seq.map (fun f -> (f, context.CancellationToken))
        |> Seq.map responseStream.WriteAsync
        |> Task.WhenAll
