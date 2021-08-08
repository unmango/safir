using System.IO;
using JetBrains.Annotations;

namespace Safir.Agent.Configuration
{
    internal class AgentOptions
    {
        public string? DataDirectory { get; set; }
        
        public bool EnableGrpcReflection { get; [UsedImplicitly] set; }
        
        public bool EnableSwagger { get; [UsedImplicitly] set; }
        
        public EnumerationOptions? EnumerationOptions { get; [UsedImplicitly] set; }

        public int MaxDepth { get; [UsedImplicitly] set; }

        public string Redis { get; set; } = string.Empty;
    }
}
