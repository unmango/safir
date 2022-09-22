using System.CommandLine.Invocation;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace Safir.CommandLine;

[PublicAPI]
public record HandlerContext(
    IConfiguration Configuration,
    InvocationContext InvocationContext,
    IServiceProvider Services);
