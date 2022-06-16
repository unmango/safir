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
open Safir.Agent.Queries
open Safir.Agent.Services

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =
        let builder =
            WebApplication.CreateBuilder(args)

        builder.Services.AddGrpc()
        builder.Services.AddGrpcReflection()

        //        builder.Services.AddControllers()

        builder
            .Services
            .AddOptions<AgentOptions>()
            .BindConfiguration(String.Empty)

        builder
            .Services
            .AddTransient<IFileSystem, FileSystem>()
            .AddTransient<IDirectory, DirectoryWrapper>()
            .AddTransient<IFile, FileWrapper>()
            .AddTransient<IPath, PathWrapper>()

        builder.Services.AddSingleton(ListFiles.ListFilesWrapper)

        let app = builder.Build()

        if app.Environment.IsDevelopment() then
            do app.UseDeveloperExceptionPage()

        //        app.UseHttpsRedirection()

        //        app.UseAuthorization()
//        app.MapControllers()
        app.MapGrpcService<FileSystemService>()
        app.MapGrpcReflectionService()

        app.Run()

        exitCode
