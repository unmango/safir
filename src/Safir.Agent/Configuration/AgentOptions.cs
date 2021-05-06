using System.IO;
using JetBrains.Annotations;

namespace Safir.Agent.Configuration
{
    internal class AgentOptions
    {
        public string? DataDirectory { get; set; }
        
        public EnumerationOptions? EnumerationOptions { get; [UsedImplicitly] set; }

        public int MaxDepth { get; [UsedImplicitly] set; }

        [UsedImplicitly]
        public string Redis { get; set; } = string.Empty;
    }
}
