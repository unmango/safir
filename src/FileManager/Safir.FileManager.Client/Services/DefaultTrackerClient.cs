using System.Threading;
using System.Threading.Tasks;

namespace Safir.FileManager.Client.Services
{
    internal class DefaultTrackerClient : Tracker.TrackerClient, ITrackerClient
    {
        public async Task<TrackResponse> TrackAsync(TrackRequest request, CancellationToken cancellationToken = default)
        {
            return await TrackAsync(request, headers: null, deadline: null, cancellationToken: cancellationToken);
        }
    }
}
