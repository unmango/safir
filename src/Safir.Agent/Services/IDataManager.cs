using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Agent.Services
{
    public interface IDataManager
    {
        Task<IEnumerable<string>> ListAsync(
            string? filterPattern = null,
            CancellationToken cancellationToken = default);
    }
}
