using System;
using System.Collections.Generic;
using System.Linq;
using Cli.Services;
using Cli.Services.Sources;
using Xunit;

namespace Cli.Tests.Helpers
{
    public class ServiceSources : TheoryData<IServiceSource>
    {
        private static readonly List<IServiceSource> _sources = new(5) {
            new DockerBuildSource("Name", "context"),
            new DockerImageSource("Name", "image"),
            new DotnetToolSource("Name", "tool"),
            new GitSource("Name", "https://example.com/repo.git"),
            new LocalDirectorySource("Name", "directory/that/does/not/exist"),
        };
        
        public ServiceSources(IEnumerable<IServiceSource>? sources = null)
        {
            foreach (var source in sources ?? _sources)
            {
                Add(source);
            }
        }

        public static ServiceSources Except(Type type)
        {
            return new(_sources.Where(x => x.GetType() != type));
        }
    }
}
