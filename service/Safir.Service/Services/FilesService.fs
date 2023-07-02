namespace Safir.Service.Services

open Safir.V1alpha1

type FilesService() =
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
