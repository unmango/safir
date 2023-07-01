namespace Safir.Service.Services

open System
open System.IO
open System.Threading.Tasks
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Safir.Service
open Safir.V1alpha1

type StartupScanner(config: IConfiguration, scopeFactory: IServiceScopeFactory) =
    inherit BackgroundService()

    let scan directory =
        use scope = scopeFactory.CreateScope()
        let service = scope.ServiceProvider.GetRequiredService<Files.Service>()

        Directory.EnumerateFiles(directory, "*", EnumerationOptions(RecurseSubdirectories = true))
        |> Seq.map (fun f -> f, Path.GetFileName(f))
        |> Seq.map (fun (f, n) ->
            service.Discovered(Files.FileId.ofGuid (Guid.NewGuid()), { File.empty () with FullPath = f; Name = n }))
        |> Async.Sequential
        |> Async.Ignore

    override this.ExecuteAsync(stoppingToken) =
        let directory = config["MediaDirectory"]

        if Directory.Exists(directory) then
            Async.StartAsTask((scan directory), cancellationToken = stoppingToken)
        else
            Task.CompletedTask
