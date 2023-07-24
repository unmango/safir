open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Propulsion.Feed
open Safir.Service
open Safir.Service.Services
open Serilog

let logger =
    LoggerConfiguration()
        .WriteTo.Console()
        .MinimumLevel.Debug()
        .CreateLogger()

Log.Logger <- logger

module DI =
    open Equinox
    open Safir.Service.Domain

    [<Literal>]
    let ConnectionName = "Safir"

    let register connectionString (services: IServiceCollection) =
        let resolve c n s = Decider.resolve logger c n s
        let connection = Config.Store.connect ConnectionName connectionString
        let store = Config.Store.create ConnectionName connection

        services
            .AddSingleton(Files.create resolve store)
            .AddSingleton(FileSystem.create resolve store)

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)
    let connectionString = builder.Configuration.GetConnectionString("EventStore")

    builder.Services
        .AddHostedService<EventStoreSource>()
        .AddSingleton<IFeedCheckpointStore>(CheckpointStore(logger, Config.checkpointInterval))
    |> DI.register connectionString
    |> ignore

    builder.Services
        .AddGrpcReflection()
        .AddGrpc()
    |> ignore

    builder.Host.UseSerilog(logger) |> ignore

    let app = builder.Build()

    app.MapGrpcReflectionService() |> ignore
    app.MapGrpcService<FilesService>() |> ignore

    app.Run()

    0 // Exit code
