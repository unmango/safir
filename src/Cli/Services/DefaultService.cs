using System.Collections.Generic;
using Cli.Services.Configuration;

namespace Cli.Services
{
    internal record DefaultService(string Name, IEnumerable<IServiceSource> Sources) : IService
    {
    }
}
