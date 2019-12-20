using System.Threading;
using System.Threading.Tasks;

namespace Safir.FileManager.Client
{
    public interface ITrackerClient
    {
        Task<TrackResponse> TrackAsync(TrackRequest request, CancellationToken cancellationToken = default);
    }
}
