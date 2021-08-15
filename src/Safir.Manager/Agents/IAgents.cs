using System.Collections.Generic;

namespace Safir.Manager.Agents
{
    internal interface IAgents : IEnumerable<IAgent>
    {
        IAgent this[string name] { get; }
    }
}
