using JetBrains.Annotations;

namespace Safir.Manager.Configuration
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ManagerOptions
    {
        public string Redis { get; set; } = string.Empty;
    }
}
