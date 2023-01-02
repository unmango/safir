using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Safir.Manager.Protos;

namespace Safir.Manager.Services;

internal sealed class MediaService : Media.MediaBase
{
    private readonly IAgents _agents;

    public MediaService(IAgents agents)
    {
        _agents = agents;
    }

    public override async Task List(Empty request, IServerStreamWriter<MediaItem> responseStream, ServerCallContext context)
    {
        var ct = context.CancellationToken;
        var items = _agents.ListFilesAsync(ct).Select(x => new MediaItem {
            Host = x.Host,
            Path = x.Entry.Path,
        });

        await foreach (var item in items.WithCancellation(ct)) {
            await responseStream.WriteAsync(item, ct);
        }
    }
}
