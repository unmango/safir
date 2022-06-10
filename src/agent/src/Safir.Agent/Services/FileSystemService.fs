namespace Safir.Agent.Services
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Safir.Agent.Configuration
open Safir.Agent.Protos

type FileSystemService(
    options: IOptions<AgentOptions>,
    logger: ILogger<FileSystemService>) =
    inherit FileSystem.FileSystemBase()

    override this.ListFiles(request, responseStream, context) =
        options.Value.DataDirectory
        |> Option.fold (fun _ s -> String.IsNullOrWhiteSpace(s))
