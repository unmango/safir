namespace Safir.Service.Services

open FSharp.Control
open Google.Protobuf
open Microsoft.Extensions.Configuration
open Safir.Service
open Safir.V1alpha1

type FilesService(service: Files.Service, config: IConfiguration) =
    inherit FilesService.FilesServiceBase()

    override this.List request context = task {
        let! files = service.List() |> AsyncSeq.toListAsync
        let res = TestResponse.empty ()
        res.Message.AddRange(files)
        return res
    }
