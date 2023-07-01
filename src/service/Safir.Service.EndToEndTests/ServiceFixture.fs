namespace Safir.Service.EndToEndTests

open System
open System.IO
open System.Threading.Tasks
open DotNet.Testcontainers.Builders
open Fake.DotNet
open Xunit

module Build =
    let slnDir =
        CommonDirectoryPath
            .GetSolutionDirectory()
            .DirectoryPath

    let projectFile = Path.Combine(slnDir, "Safir.Service", "Safir.Service.fsproj")

module Profile =
    let container (defaults: MSBuildParams) = {
        defaults with
            ToolPath = "dotnet msbuild"
            Targets = [ "Publish" ]
            Properties = [
                "Configuration", "Release"
                "PublishProfile", "DefaultContainer"
                "ContainerRuntimeIdentifier", "linux-x64"
                "ContainerImageTag", "safir-service-e2e"
            ]
            Verbosity = Some(Diagnostic)
    }

type ServiceFixture() =
    interface IAsyncLifetime with
        member this.InitializeAsync() =
            try
                MSBuild.build Profile.container Build.projectFile
            with MSBuildException(m, ms) ->
                raise (Exception(MSBuild.msBuildExe))

            Task.CompletedTask

        member this.DisposeAsync() = Task.CompletedTask
