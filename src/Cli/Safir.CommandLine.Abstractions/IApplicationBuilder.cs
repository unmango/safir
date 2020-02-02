using System.Collections.Generic;
using System.CommandLine.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace System.CommandLine
{
    public interface IApplicationBuilder
    {
        CommandLineBuilder ParserBuilder { get; }

        IDictionary<object, object> Properties { get; }

        ICommandLineApplication Build();

        IApplicationBuilder ConfigureConfiguration(Action<ApplicationBuilderContext, IConfigurationBuilder> configure);

        IApplicationBuilder ConfigureServices(Action<ApplicationBuilderContext, IServiceCollection> configure);
    }
}
