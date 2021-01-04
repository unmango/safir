using System.Linq;

namespace Cli.Services
{
    internal static class ServiceEntryExtensions
    {
        public static IServiceInstaller GetInstaller(this ServiceEntry service)
        {
            return service.Sources.OrderByPriority().Single().GetInstaller();
        }

        public static IServiceInstaller? GetGitInstaller(this ServiceEntry service)
        {
            return service.Sources.HighestPriorityOrDefault(x => x.Type == SourceType.Git)?.GetGitInstaller();
        }

        public static IServiceInstaller? GetDotnetToolInstaller(this ServiceEntry service)
        {
            return service.Sources.HighestPriorityOrDefault(x => x.Type == SourceType.DotnetTool)
                ?.GetDotnetToolInstaller();
        }

        public static IServiceInstaller? GetLocalDirectoryInstaller(this ServiceEntry service)
        {
            return service.Sources.HighestPriorityOrDefault(x => x.Type == SourceType.LocalDirectory)
                ?.GetLocalDirectoryInstaller();
        }
    }
}
