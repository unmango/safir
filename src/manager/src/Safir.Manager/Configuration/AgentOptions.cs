using JetBrains.Annotations;

namespace Safir.Manager.Configuration
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AgentOptions
    {
        private string? _name;
        
        public string BaseUrl { get; set; } = string.Empty;

        public string Name
        {
            get => _name ?? BaseUrl;
            set => _name = value;
        }
    }
}
