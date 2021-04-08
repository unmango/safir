using JetBrains.Annotations;

namespace Safir.Agent.Configuration
{
    public class AgentOptions
    {
        public string? DataDirectory { get; set; }

        public int MaxDepth { get; [UsedImplicitly] set; }
    }
}
