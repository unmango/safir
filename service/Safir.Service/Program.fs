open EventStore.Client
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Safir.Service
open Safir.Service.Services

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)
    let connectionString = builder.Configuration.GetConnectionString("EventStore")

    builder.Services
        .AddGrpcReflection()
        .AddGrpc()
    |> ignore

    builder.Services
        .AddScoped<EventStoreClient>(fun _ -> Config.Store.connect connectionString)
        .AddScoped<Files.Service>()
    |> ignore

    let app = builder.Build()

    app.MapGrpcReflectionService() |> ignore
    app.MapGrpcService<FilesService>() |> ignore

    app.Run()

    0 // Exit code
