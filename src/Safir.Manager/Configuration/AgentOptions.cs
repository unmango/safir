using JetBrains.Annotations;

namespace Safir.Manager.Configuration
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AgentOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
    }
}
