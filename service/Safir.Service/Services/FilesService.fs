namespace Safir.Service.Services

open Safir.Service
open Safir.V1alpha1

type FilesService(service: Files.Service) =
    inherit FilesService.FilesServiceBase()

    override this.Discover request context = task {
        let! view = service.Discover(request.Name, request.FullPath, context.CancellationToken)

        let file = {
            File.empty () with
                Id = view.Id
                Name = request.Name
                FullPath = request.FullPath
                Sha256 = request.Sha256
        }

        return {
            FilesServiceDiscoverResponse.empty () with
                File = ValueSome file
        }
    }

    override this.List request context = task {
        let! results = service.List(context.CancellationToken)
        let response = FilesServiceListResponse.empty ()

        response.Files.AddRange(
            [
                for file in results do
                    yield {
                        File.empty () with
                            Id = file.Id
                            Name = file.Name
                            FullPath = file.Path
                    }
            ]
        )

        return response
    }
