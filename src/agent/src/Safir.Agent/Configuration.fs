namespace Safir.Agent.Configuration

open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Options
open Safir.Messaging.Configuration

type AgentOptions() =
    member val DataDirectory: string = null with get, set
    member val EnableGrpcReflection = false with get, set
    member val EnableSwagger = false with get, set
    member val MaxDepth = 0 with get, set
    member val Redis = "" with get, set

type SafirMessaging(configuration: IConfiguration) =
    interface IConfigureOptions<MessagingOptions> with
        member this.Configure(options) =
            options.ConnectionString <- configuration.Get<AgentOptions>().Redis
