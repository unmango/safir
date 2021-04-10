using Akka.Actor;
using Microsoft.Extensions.Options;
using Safir.Agent.Configuration;
using Safir.Agent.Domain;

namespace Safir.Agent.Actors
{
    public class DataDirectoryActor : ReceiveActor
    {
        public record List;
        
        private readonly string _root;
        private readonly IDirectory _directory;
        private readonly IOptionsMonitor<AgentOptions> _options;

        public DataDirectoryActor(string root, IDirectory directory, IOptionsMonitor<AgentOptions> options)
        {
            _root = root;
            _directory = directory;
            _options = options;
        }

        private void OnList(List message)
        {
            var options = _options.CurrentValue.EnumerationOptions;
            var entries = _directory.EnumerateFileSystemEntries(_root, "*", options);
            // TODO: protobuf types?
            Sender.Tell(entries);
        }
    }
}
