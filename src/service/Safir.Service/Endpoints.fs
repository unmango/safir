module Safir.Service.Endpoints

open Giraffe

let get: HttpHandler =
    handleContext (fun context -> task {
        let service = context.GetService<Library.Service>()
        let! view = service.List("yeet")
        return! context.WriteJsonAsync(view)
    })
