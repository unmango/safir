using System.Collections.Generic;
using System.CommandLine;
using System.Linq;

namespace Cli.Commands.Service
{
    internal sealed class ServiceArgument : Argument<IEnumerable<string>>
    {
        private const string DefaultDescription = "The name of the service the manage";
        
        private static readonly string[] _services = {
            "manager",
            "listener",
        };

        public ServiceArgument(string description) : this(null, description)
        {
        }
        
        public ServiceArgument(IEnumerable<string>? services = null, string? description = null)
            : base("service", description ?? DefaultDescription)
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
