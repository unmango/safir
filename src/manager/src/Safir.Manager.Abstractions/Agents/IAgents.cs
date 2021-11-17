using System.Collections.Generic;
using JetBrains.Annotations;

namespace Safir.Manager.Agents
{
    [PublicAPI]
    public interface IAgents : IEnumerable<IAgent>
    {
        IAgent this[string name] { get; }
    }
}
