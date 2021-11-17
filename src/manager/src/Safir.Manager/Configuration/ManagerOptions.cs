using System.Collections.Generic;
using JetBrains.Annotations;

namespace Safir.Manager.Configuration
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ManagerOptions
    {
        public List<AgentOptions> Agents { get; set; } = new();
        
        public bool EnableGrpcReflection { get; set; }
        
        public bool EnableSwagger { get; set; }
        
        public bool ProxyAgent { get; set; }

        public string ProxyDataDirectory { get; set; } = string.Empty;

        public string Redis { get; set; } = string.Empty;
    }
}
