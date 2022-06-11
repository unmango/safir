namespace Safir.Agent.Services
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Safir.Agent.Configuration
open Safir.Agent.Protos
open Safir.Agent.Queries.ListFiles

type FileSystemService(
    options: IOptions<AgentOptions>,
    logger: ILogger<FileSystemService>) =
    inherit FileSystem.FileSystemBase()

    override this.ListFiles(_, responseStream, _) =
        task { return listFiles options logger }
