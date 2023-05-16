namespace Safir.Service.Services

open System.IO
open System.Threading.Tasks
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Safir.Service

type StartupScanner(config: IConfiguration, scopeFactory: IServiceScopeFactory) =
    let scan directory =
        use scope = scopeFactory.CreateScope()
        let service = scope.ServiceProvider.GetRequiredService<Library.Service>()

        Directory.EnumerateFileSystemEntries directory
        |> Seq.map (fun e -> service.Discover("yeet", e))
        |> Async.Parallel

    interface IHostedService with
        member this.StartAsync(cancellationToken) =
            let directory = config["MediaDirectory"]

            if Directory.Exists(directory) then
                Async.StartAsTask((scan directory), cancellationToken = cancellationToken)
            else
                Task.CompletedTask

        member this.StopAsync _ = Task.CompletedTask
