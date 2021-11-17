using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Safir.Agent.Protos;
using Safir.Manager.Protos;

namespace Safir.Manager.Agents
{
    internal class AgentAggregator
    {
        private readonly IEnumerable<IAgent> _agents;

        public AgentAggregator(IEnumerable<IAgent> agents)
        {
            _agents = agents ?? throw new ArgumentNullException(nameof(agents));
        }

        public IAsyncEnumerable<MediaItem> List(CancellationToken cancellationToken)
        {
            return _agents.ToAsyncEnumerable()
                .SelectMany(x => x.FileSystem.ListFilesAsync(cancellationToken), ToMedia);

            static MediaItem ToMedia(IAgent agent, FileSystemEntry entry) => new() {
                Host = agent.Name,
                Path = entry.Path,
            };
        }
    }
}
