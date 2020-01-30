using System.Threading;
using System.Threading.Tasks;

namespace Safir.Common.Data
{
    public interface IUnitOfWork
    {
        ValueTask SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
