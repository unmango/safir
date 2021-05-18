using System;
using JetBrains.Annotations;

namespace Safir.Manager.Domain
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.WithMembers)]
    public abstract record Entity
    {
        public long Id { get; protected set; }
        
        public DateTime Created { get; protected set; }
        
        public DateTime Updated { get; protected set; }
    }
}
