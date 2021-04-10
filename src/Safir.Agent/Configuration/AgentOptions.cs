using System.IO;
using JetBrains.Annotations;

namespace Safir.Agent.Configuration
{
    internal class AgentOptions
    {
        public string? DataDirectory { get; set; }

        public int MaxDepth { get; [UsedImplicitly] set; }
        
        public EnumerationOptions? EnumerationOptions { get; [UsedImplicitly] set; }
    }
}
