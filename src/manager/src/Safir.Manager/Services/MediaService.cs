using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Safir.Agent.Protos;
using Safir.Grpc;
using Safir.Manager.Agents;
using Safir.Manager.Protos;

namespace Safir.Manager.Services
{
    internal class MediaService : Media.MediaBase
    {
        private readonly AgentAggregator _aggregator;

        public MediaService(AgentAggregator aggregator)
        {
            _aggregator = aggregator ?? throw new ArgumentNullException(nameof(aggregator));
        }

        public override Task List(Empty request, IServerStreamWriter<MediaItem> responseStream, ServerCallContext context)
        {
            var media = _aggregator.List(context.CancellationToken);
            return responseStream.WriteAllAsync(media);
        }
    }
}
