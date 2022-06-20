namespace Safir.Agent.Services

open System.Threading.Tasks
open Safir.Agent.Protos
open Safir.Agent.Queries.ListFiles

type FileSystemService(query: ListFiles) =
    inherit FileSystem.FileSystemBase()

    override this.ListFiles(_, responseStream, context) =
        query.Execute()
        |> Seq.map (fun f -> (f, context.CancellationToken))
        |> Seq.map responseStream.WriteAsync
        |> Task.WhenAll
