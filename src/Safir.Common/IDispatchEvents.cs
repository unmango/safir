using System.Collections.Generic;
using Safir.Common.Domain;

namespace Safir.Common
{
    public interface IDispatchEvents
    {
        IEnumerable<Entity> GetEntities();
    }
}
