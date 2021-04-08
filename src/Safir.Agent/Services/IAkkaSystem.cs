using Akka.Actor;

namespace Safir.Agent.Services
{
    public interface IAkkaSystem
    {
        ActorSystem System { get; }
    }
}
