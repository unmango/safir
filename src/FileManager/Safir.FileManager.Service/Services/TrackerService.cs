using System.Threading.Tasks;
using Grpc.Core;

namespace Safir.FileManager.Service.Services
{
    internal class TrackerService : Tracker.TrackerBase
    {
        public override Task<TrackResponse> Track(TrackRequest request, ServerCallContext context)
        {
            return base.Track(request, context);
        }
    }
}
