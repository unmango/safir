using System.CommandLine;

namespace Cli.Commands.Service
{
    internal sealed class ServiceOption : Option
    {
        private static readonly string[] _services = {
            "listener",
            "all"
        };

        public static readonly ServiceOption Value = new();
        
        public ServiceOption() : base(
            new[] { "--service", "-s" },
            "The service the modify")
        {
            IsRequired = true;
            Argument = new Argument<string>("name", "The name of the service") {
                Arity = new ArgumentArity(1, _services.Length),
            };

            Argument.FromAmong(_services);
            Argument.AddSuggestions(_services);
        }
    }
}
