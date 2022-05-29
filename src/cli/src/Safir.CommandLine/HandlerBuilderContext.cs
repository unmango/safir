using System.CommandLine.Invocation;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace Safir.CommandLine;

[PublicAPI]
public class HandlerBuilderContext
{
    public HandlerBuilderContext(IConfiguration configuration, InvocationContext invocationContext)
    {
        Configuration = configuration;
        InvocationContext = invocationContext;
    }

    public IConfiguration Configuration { get; set; }

    public InvocationContext InvocationContext { get; }
}
