namespace Safir.Service.Services

open Safir.Service.Domain
open Safir.Service.Domain.Files
open Safir.V1alpha1

type FilesService(service: Files.Service) =
    inherit FilesService.FilesServiceBase()

    override this.List request context = task {
        let response = FilesServiceListResponse.empty ()

        response.Files.AddRange(
            [
                { File.empty () with Name = "Test"; FullPath = "TestPath" }
            ]
        )

        return response
    }

    override this.Discover request _ = task {
        let file: Events.File = {
            FullPath = request.FullPath
            Sha256 = request.Sha256
            Name = request.Name
        }

        let id = FileId.gen ()
        do! service.Discover(id, file)

        return {
            FilesServiceDiscoverResponse.empty () with
                File =
                    ValueSome {
                        File.empty () with
                            Id = FileId.toString id
                            Name = file.Name
                            Sha256 = file.Sha256
                            FullPath = file.FullPath
                    }
        }
    }
