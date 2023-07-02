open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Safir.Service.Services

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)

    builder.Services
        .AddGrpcReflection()
        .AddGrpc()
    |> ignore

    let app = builder.Build()

    app.MapGrpcReflectionService() |> ignore
    app.MapGrpcService<FilesService>() |> ignore

    app.Run()

    0 // Exit code
