using System.Collections.Generic;

namespace Cli.Services.Installation
{
    internal interface IServiceInstalled
    {
        bool Installed { get; }
        
        IEnumerable<ISourceInstalled> Sources { get; }
    }
}
