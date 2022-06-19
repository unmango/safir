namespace Safir.Agent.Configuration

type AgentOptions() =
    member val DataDirectory: string = null with get, set
    member val EnableGrpcReflection = false with get, set
    member val EnableSwagger = false with get, set
    member val MaxDepth = 0 with get, set
