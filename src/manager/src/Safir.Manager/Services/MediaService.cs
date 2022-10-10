using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Safir.Manager.Protos;

namespace Safir.Manager.Services;

internal sealed class MediaService : Media.MediaBase
{
    public override Task List(Empty request, IServerStreamWriter<MediaItem> responseStream, ServerCallContext context)
    {
        return responseStream.WriteAsync(new() { Host = "Test", Path = "Test" });
    }
}
