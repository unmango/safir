using System.Collections.Generic;
using System.CommandLine;
using System.Linq;

namespace Cli.Commands.Service
{
    internal sealed class ServiceOption : Option
    {
        private static readonly string[] _services = {
            "manager",
            "listener",
        };
        
        public ServiceOption(bool required = true, IEnumerable<string>? services = null) : base(
            new[] { "--service", "-s" },
            "The service the modify")
        {
            IsRequired = required;
            Argument = new Argument<string>("name", "The name of the service") {
                Arity = ArgumentArity.OneOrMore,
            };

            var available = GetServices(services);
            Argument.FromAmong(available);
            Argument.AddSuggestions(available);
        }

        private static string[] GetServices(IEnumerable<string>? extra)
        {
            return extra == null ? _services : _services.Union(extra).ToArray();
        }
    }
}
