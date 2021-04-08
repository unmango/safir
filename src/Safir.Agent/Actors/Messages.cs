using Akka.Actor;
using Safir.Agent.Configuration;

namespace Safir.Agent.Actors
{
    public record Subscribe(IActorRef Actor);

    public record Unsubscribe(IActorRef Actor);

    public record OptionsValue(AgentOptions Options);

    public record OptionsChange(AgentOptions Options);

    public record GetOptionsValue;

    public record GetDataDirectory;

    public record DataDirectoryChange(string Path, string? OldPath);
}
