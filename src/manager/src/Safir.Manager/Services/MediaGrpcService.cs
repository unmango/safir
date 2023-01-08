using Grpc.Core;
using Safir.Manager.V1Alpha1;

namespace Safir.Manager.Services;

internal sealed class MediaGrpcService : MediaService.MediaServiceBase
{
    private readonly IAgents _agents;

    public MediaGrpcService(IAgents agents)
    {
        _agents = agents;
    }

    public override async Task<MediaServiceListResponse> List(MediaServiceListRequest request, ServerCallContext context)
    {
        var ct = context.CancellationToken;
        var items = await _agents.ListFilesAsync(ct)
            .Select(x => new MediaItem {
                Host = x.Host,
                Path = x.Entry.Path,
            })
            .ToListAsync(cancellationToken: ct);

        return new MediaServiceListResponse {
            Media = { items },
        };
    }
}
