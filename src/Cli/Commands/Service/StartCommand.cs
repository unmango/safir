using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Cli.Services;
using Microsoft.Extensions.Options;

namespace Cli.Commands.Service
{
    internal sealed class StartCommand : Command
    {
        private static readonly ServiceOption _serviceOption = new();
        private static readonly AllServicesOption _allServicesOption = new();
        
        public StartCommand() : base("start", "Start the selected service(s)")
        {
            AddOption(_serviceOption);
            AddOption(_allServicesOption);
        }
        
        // ReSharper disable once ClassNeverInstantiated.Global
        public sealed class StartHandler : ICommandHandler
        {
            // private readonly IOptions<Options> _options;
            // private readonly IServiceRegistry _services;
            //
            // public StartHandler(IOptions<Options> options, IServiceRegistry services)
            // {
            //     _options = options ?? throw new ArgumentNullException(nameof(options));
            //     _services = services ?? throw new ArgumentNullException(nameof(services));
            // }
            
            public Task<int> InvokeAsync(InvocationContext context)
            {
                var requested = context.ParseResult.ValueForOption<string[]>(_serviceOption);
                
                throw new System.NotImplementedException();
            }
        }
    }
}
