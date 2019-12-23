using System.Threading;
using System.Threading.Tasks;

namespace Safir.Common.Data
{
    public static class RepositoryExtensions
    {
        public static ValueTask SaveChangesAsync(
            this IRepository repository,
            CancellationToken cancellationToken = default)
        {
            return repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
