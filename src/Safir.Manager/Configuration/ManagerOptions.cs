using JetBrains.Annotations;

namespace Safir.Manager.Configuration
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ManagerOptions
    {
        public string DataDirectory { get; set; } = string.Empty;
        
        public bool IsSelfContained { get; set; }
        
        public string Redis { get; set; } = string.Empty;
    }
}
