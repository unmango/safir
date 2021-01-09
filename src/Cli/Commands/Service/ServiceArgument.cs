using System.Collections.Generic;
using System.CommandLine;
using System.Linq;

namespace Cli.Commands.Service
{
    internal sealed class ServiceArgument : Argument<string>
    {
        private static readonly string[] _services = {
            "manager",
            "listener",
        };
        
        public ServiceArgument(IEnumerable<string>? services = null)
            : base("service", "The name of the service the modify")
        {
            Arity = ArgumentArity.OneOrMore;

            var available = GetServices(services);
            this.FromAmong(available);
            this.AddSuggestions(available);
        }

        private static string[] GetServices(IEnumerable<string>? extra)
        {
            return extra == null ? _services : _services.Union(extra).ToArray();
        }
    }
}
