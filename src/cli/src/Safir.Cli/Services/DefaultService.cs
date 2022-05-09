using System.Collections.Generic;

namespace Safir.Cli.Services;

internal record DefaultService(string Name, IEnumerable<IServiceSource> Sources) : IService;
