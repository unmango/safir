open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Safir.Service
open Safir.Service.Services
open Serilog

let logger =
    LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger()

Log.Logger <- logger

module DI =
    open Equinox

    let register connectionString create (services: IServiceCollection) =
        let resolve = Decider.resolve logger
        let store = Config.connect connectionString
        let service: 's = create resolve store
        services.AddSingleton<'s>(service)

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)

    let connectionString = builder.Configuration.GetConnectionString("EventStore")
    let register = DI.register connectionString

    builder.Services |> register Files.create |> ignore

    builder.Services
        .AddGrpcReflection()
        .AddGrpc()
    |> ignore

    builder.Host.UseSerilog() |> ignore

    let app = builder.Build()

    app.MapGrpcReflectionService() |> ignore
    app.MapGrpcService<FilesService>() |> ignore

    app.Run()

    0 // Exit code
