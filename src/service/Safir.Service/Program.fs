open EventStore.Client
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Propulsion.EventStoreDb
open Safir.Service
open Safir.Service.Services
open System

module private ES =
    open Equinox.EventStoreDb

    let connect connectionString =
        let connector = EventStoreConnector(reqTimeout = TimeSpan.FromSeconds(5), reqRetries = 1)
        let strategy = ConnectionStrategy.ClusterTwinPreferSlaveReads
        let connection = connector.Establish("Twin", Discovery.ConnectionString connectionString, strategy)
        EventStoreContext(connection, batchSize = 500)

    let client connectionString =
        let settings = EventStoreClientSettings.Create(connectionString)
        new EventStoreClient(settings)

module private DI =
    let register (services: IServiceCollection) (options: Config.Options) =
        let cache = Equinox.Cache("EventStore", options.CacheMb)
        let context = ES.connect options.ConnectionStrings.EventStore
        let client = ES.client options.ConnectionStrings.EventStore

        services
            // .AddSingleton(Library.Service.create (context, cache))
            .AddSingleton(Files.Service.create (context, cache))
            .AddSingleton(client)

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)

    let options = builder.Configuration.Get<Config.Options>()
    DI.register builder.Services options |> ignore

    builder.Services
        .AddGrpcReflection()
        .AddGrpc()
    |> ignore

    builder.Services
        .AddHostedService<StartupScanner>()
        // .AddHostedService<DataWatcher>()
    |> ignore

    let app = builder.Build()

    app.MapGrpcReflectionService() |> ignore
    app.MapGrpcService<FilesService>() |> ignore

    app.Run()

    0 // Exit code
