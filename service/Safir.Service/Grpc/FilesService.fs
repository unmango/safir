namespace Safir.Service.Services

open Safir.Service.Domain
open Safir.V1alpha1

type FilesService(files: Files.Service, fileSystem: FileSystem.Service) =
    inherit FilesService.FilesServiceBase()

    override this.List request context = task {
        let! ids = fileSystem.Query(FileSystem.id)

        let! items =
            ids
            |> List.map (fun i -> async {
                let! file = files.Query(i)

                return
                    file
                    |> Option.map (fun x -> {
                        File.empty () with
                            Id = Files.FileId.toString i
                            Name = x.File.Name
                            FullPath = x.File.FullPath
                            Sha256 = x.File.Sha256
                    })
                    |> Option.defaultValue (File.empty ())
            })
            |> Async.Parallel

        let response = FilesServiceListResponse.empty ()
        response.Files.AddRange(items)
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
