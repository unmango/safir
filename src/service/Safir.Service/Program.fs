open Giraffe
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
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

module private DI =
    let register (services: IServiceCollection) (options: Config.Options) =
        let cache = Equinox.Cache("EventStore", options.CacheMb)
        let context = ES.connect options.ConnectionStrings.EventStore

        services
            .AddSingleton(Library.Service.create (context, cache))
            .AddSingleton(FileSystem.Service.create (context, cache))

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)

    builder.Services.AddGiraffe() |> ignore

    let options = builder.Configuration.Get<Config.Options>()
    DI.register builder.Services options |> ignore

    builder.Services.AddHostedService<StartupScanner>() |> ignore

    let app = builder.Build()

    app.UseGiraffe(choose [ route "/" >=> Endpoints.get ])

    app.Run()

    0 // Exit code
