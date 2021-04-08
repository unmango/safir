using Akka.Actor;
using Akka.DependencyInjection;

namespace Safir.Agent.Services
{
    internal static class AkkaSystemExtensions
    {
        public static Props Props<T>(this IAkkaSystem system, params object[] args)
            where T : ActorBase
        {
            return ServiceProvider.For(system.System).Props<T>(args);
        }
    }
}
