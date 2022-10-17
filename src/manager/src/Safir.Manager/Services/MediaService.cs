using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Options;
using Safir.Agent.Client;
using Safir.Agent.Protos;
using Safir.Manager.Protos;

namespace Safir.Manager.Services;

internal sealed class MediaService : Media.MediaBase
{
    private readonly IDictionary<string, FileSystem.FileSystemClient> _clients;

    public MediaService(IOptions<ManagerConfiguration> options, GrpcClientFactory clientFactory)
    {
        var agents = options.Value.GetAgentOptions();
        _clients = agents.ToDictionary(
            x => x.Name,
            x => clientFactory.CreateClient<FileSystem.FileSystemClient>(x.Name));
    }

    public override async Task List(Empty request, IServerStreamWriter<MediaItem> responseStream, ServerCallContext context)
    {
        var ct = context.CancellationToken;
        var files = _clients.ToAsyncEnumerable().SelectMany(
            x => x.Value.ListFilesAsync(ct),
            (pair, entry) => new { Name = pair.Key, entry.Path });

        await foreach (var file in files.WithCancellation(ct)) {
            await responseStream.WriteAsync(new() { Host = "", Path = file.Path }, ct);
        }
    }
}
