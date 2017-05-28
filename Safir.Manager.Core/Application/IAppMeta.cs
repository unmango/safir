using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.Core.Application
{
    public interface IAppMeta
    {
        string AppName { get; }
        string AppVersion { get; }
    }
}
