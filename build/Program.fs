open Fake.Core
open Fake.DotNet
open Fake.IO.FileSystemOperators

let rootDir = __SOURCE_DIRECTORY__ </> ".."
let serviceDir = rootDir </> "service"
let serviceProject = serviceDir </> "Safir.Service" </> "Safir.Service.fsproj"

let sln = rootDir </> "Safir.sln"

let buildEnv =
    [
        "SuppressNETCoreSdkPreviewMessage", "true"
        "PATH", "/usr/bin" // Hack to get SdkContainers to find the docker CLI
    ]
    |> Map

let build _ =
    DotNet.build
        (fun options -> {
            options with
                Configuration = DotNet.BuildConfiguration.Release
                Common = options.Common |> DotNet.Options.withEnvironment buildEnv
        })
        sln

let buildDocker _ =
    DotNet.publish
        (fun options -> {
            options with
                Configuration = DotNet.BuildConfiguration.Release
                Common =
                    options.Common
                    |> DotNet.Options.withEnvironment buildEnv
                    |> DotNet.Options.withAdditionalArgs [
                        "--os"
                        "linux"
                        "--arch"
                        "x64"
                        "/p:PublishProfile=DefaultContainer"
                    ]
        })
        serviceProject

let generateDockerfile _ =
    let dir = serviceDir </> "Safir.Service"

    DotNet.exec (fun o -> { o with WorkingDirectory = dir }) "build-image" "--as-file Dockerfile"
    |> ignore

let initTargets () =
    Target.create "Build" build
    Target.create "BuildDocker" buildDocker
    Target.create "GenDockerfile" generateDockerfile

[<EntryPoint>]
let main argv =
    argv
    |> Array.toList
    |> Context.FakeExecutionContext.Create false "build.fsx"
    |> Context.RuntimeContext.Fake
    |> Context.setExecutionContext

    initTargets ()
    Target.runOrList ()

    0
