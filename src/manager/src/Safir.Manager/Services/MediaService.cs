using Grpc.Core;
using Safir.Manager.V1Alpha1;

namespace Safir.Manager.Services;

internal sealed class MediaService : V1Alpha1.MediaService.MediaServiceBase
{
    private readonly IAgents _agents;

    public MediaService(IAgents agents)
    {
        _agents = agents;
    }

    public override async Task<ListResponse> List(ListRequest request, ServerCallContext context)
    {
        var ct = context.CancellationToken;
        var items = await _agents.ListFilesAsync(ct)
            .Select(x => new MediaItem {
                Host = x.Host,
                Path = x.Entry.Path,
            })
            .ToListAsync(cancellationToken: ct);

        return new ListResponse {
            Media = { items },
        };
    }
}
