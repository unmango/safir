using Safir.Agent.Protos;
using Safir.EventSourcing;
using Safir.Messaging;

namespace Safir.Manager.Domain
{
    public record File : Aggregate
    {
        public string Path { get; set; } = string.Empty;

        public string Host { get; set; } = string.Empty;

        public long Length { get; set; }

        protected override void Apply(IEvent @event)
        {
            Apply((dynamic)@event);
            TryUpdateVersion(@event);
        }

        private void Apply(FileCreated created)
        {
            Path = created.Path;
            Host = created.Host;
        }
    }
}
