using JetBrains.Annotations;

namespace Safir.Messaging.Configuration
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class MessagingOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
    }
}
