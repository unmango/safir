namespace Safir.Agent

#nowarn "20"

open System
open System.IO.Abstractions
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Options
open Safir.Agent.Configuration
open Safir.Agent.GrpcServices
open Safir.Agent.Queries.ListFiles
open Safir.Agent.Services
open Safir.Messaging.DependencyInjection
open Serilog
open Serilog.Events

module Program =
    let exitCode = 0

    let configureSerilog (configuration: LoggerConfiguration) =
        configuration
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate = "[{SourceContext:1} {Level:u3}] {Message:lj}{NewLine}{Exception}")

    [<EntryPoint>]
    let main args =
        Log.Logger <-
            (configureSerilog (LoggerConfiguration()))
                .CreateBootstrapLogger()

        let builder =
            WebApplication.CreateBuilder(args)

        builder.Host.UseSerilog(fun context services configuration ->
            (configureSerilog configuration)
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
            |> ignore)

        builder.Services.AddGrpc()
        builder.Services.AddGrpcHttpApi()
        builder.Services.AddGrpcReflection()
        builder.Services.AddGrpcSwagger()
        builder.Services.AddCors()
        builder.Services.AddSwaggerGen()

        builder
            .Services
            .AddSafirMessaging()
            .ConfigureOptions<SafirMessaging>()

        builder
            .Services
            .AddTransient<IFileSystem, FileSystem>()
            .AddTransient<IDirectory, DirectoryWrapper>()
            .AddTransient<IFile, FileWrapper>()
            .AddTransient<IPath, PathWrapper>()

        builder
            .Services
            .AddOptions<AgentOptions>()
            .BindConfiguration(String.Empty)

        builder
            .Services
            .AddSingleton<DataDirectoryWatcher>()
            .AddHostedService(fun s -> s.GetRequiredService<DataDirectoryWatcher>())
            .AddSingleton<IFileWatcher>(fun s -> s.GetRequiredService<DataDirectoryWatcher>() :> IFileWatcher)
            .AddHostedService<FileEventPublisher>()
            .AddTransient<ListFiles>()

        let app = builder.Build()

        if app.Environment.IsDevelopment() then
            do app.UseDeveloperExceptionPage()

        app.UseSerilogRequestLogging()

        let options =
            app
                .Services
                .GetRequiredService<IOptions<AgentOptions>>()
                .Value

        if app.Environment.IsDevelopment() || options.EnableSwagger then
            do app.UseSwagger()
            do app.UseSwaggerUI()

        app.UseGrpcWeb(GrpcWebOptions(DefaultEnabled = true))

        app.UseCors (fun builder ->
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding")
            |> ignore)

        [ app.MapGrpcService<FileSystemService>()
          app.MapGrpcService<HostService>() ]
        |> Seq.iter (fun s -> s.RequireCors("AllowAll") |> ignore)

        if app.Environment.IsDevelopment() || options.EnableGrpcReflection then
            do app.MapGrpcReflectionService()

        app.MapGet("/config", fun context -> context.Response.WriteAsJsonAsync(options))

        app.Run()

        exitCode
