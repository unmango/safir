using System;
using System.Collections.Generic;
using Cli.Services.Configuration;

namespace Cli.Services
{
    internal static class ServiceEntryExtensions
    {
        public static IService GetService(this ServiceEntry entry, IEnumerable<IServiceSource> sources)
        {
            if (string.IsNullOrWhiteSpace(entry.Name))
                throw new InvalidOperationException("entry.Name must have a value");
            
            return new DefaultService(entry.Name, sources);
        }
    }
}
