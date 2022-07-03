namespace Safir.Agent

#nowarn "20"

open System
open System.Collections.Generic
open System.IO
open System.IO.Abstractions
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Safir.Agent.Configuration
open Safir.Agent.GrpcServices
open Safir.Agent.Queries
open Safir.Agent.Queries.ListFiles
open Safir.Agent.Services
open Safir.Messaging.DependencyInjection

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =
        let builder =
            WebApplication.CreateBuilder(args)

        builder.Services.AddGrpc()
        builder.Services.AddGrpcReflection()
        builder.Services.AddSafirMessaging()

        builder
            .Services
            .AddTransient<IFileSystem, FileSystem>()
            .AddTransient<IDirectory, DirectoryWrapper>()
            .AddTransient<IFile, FileWrapper>()
            .AddTransient<IPath, PathWrapper>()

        builder.Services.Configure<SafirMessaging>()

        builder
            .Services
            .AddOptions<AgentOptions>()
            .BindConfiguration(String.Empty)

        builder
            .Services
            .AddSingleton<DataDirectoryWatcher>()
            .AddHostedService(fun s -> s.GetRequiredService<DataDirectoryWatcher>())
            .AddSingleton<IFileWatcher>(fun s -> s.GetRequiredService<DataDirectoryWatcher>() :> IFileWatcher)
            .AddTransient<ListFiles>()

        let app = builder.Build()

        if app.Environment.IsDevelopment() then
            do app.UseDeveloperExceptionPage()

        let options =
            app
                .Services
                .GetRequiredService<IOptions<AgentOptions>>()
                .Value

//        if app.Environment.IsDevelopment() || options.EnableSwagger then
//            do app.UseSwagger()
//            do app.UseSwaggerUI()

        app.UseCors (fun b ->
            b
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding")
            |> ignore)

        app
            .MapGrpcService<FileSystemService>()
            .RequireCors("AllowAll")

        if app.Environment.IsDevelopment()
           || options.EnableGrpcReflection then
            do app.MapGrpcReflectionService()

        app.Run()

        exitCode
