using Microsoft.Extensions.Options;
using Safir.Cli.Configuration;

namespace Safir.Cli.Services;

internal sealed class DefaultAgents : IAgents
{
    private readonly SafirOptions _options;

    public DefaultAgents(IOptions<SafirOptions> options)
    {
        _options = options.Value;
    }

    public IAgent GetAgent(string name)
    {
        throw new System.NotImplementedException();
    }
}
