namespace Safir.Service.Services

open Safir.Service.Domain
open Safir.V1alpha1

type FilesService(files: Files.Service, fileSystem: FileSystem.Service) =
    inherit FilesService.FilesServiceBase()

    override this.List request context = task {
        let! items = fileSystem.Query(FileSystem.id)

        let response = FilesServiceListResponse.empty ()

        response.Files.AddRange(
            items
            |> List.map (fun x -> {
                File.empty () with
                    Id = Files.FileId.toString x
                    Name = "TODO"
                    FullPath = "TestPath"
            })
        )

        return response
    }

    override this.Discover request _ = task {
        let file: Files.Events.File = {
            FullPath = request.FullPath
            Sha256 = request.Sha256
            Name = request.Name
        }

        let id = Files.FileId.gen ()
        do! files.Discover(id, file)

        return {
            FilesServiceDiscoverResponse.empty () with
                File =
                    ValueSome {
                        File.empty () with
                            Id = Files.FileId.toString id
                            Name = file.Name
                            Sha256 = file.Sha256
                            FullPath = file.FullPath
                    }
        }
    }
