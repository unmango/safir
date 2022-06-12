namespace Safir.Agent.Services

open System.IO
open System.Threading.Tasks
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Safir.Agent.Configuration
open Safir.Agent.Configuration.ConfigurationTypes
open Safir.Agent.Protos
open Safir.Agent.Queries.ListFiles

type FileSystemService internal (options: IOptions<AgentOptions>, logger: ILogger<FileSystemService>, strategy) =
    inherit FileSystem.FileSystemBase()

    new(options, logger) = FileSystemService(options, logger, listFiles)

    override this.ListFiles(_, responseStream, context) =
        options.Value.DataDirectory
        |> DataDirectory.parse
        |> function
            | Ok x -> strategy x Directory.EnumerateFileSystemEntries
            | Error x ->
                logger.LogInformation(x)
                []
        |> Seq.map (fun f -> (f, context.CancellationToken))
        |> Seq.map responseStream.WriteAsync
        |> Task.WhenAll
