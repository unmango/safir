using LibGit2Sharp;

namespace Cli.Internal.Wrappers.Git
{
    public class LibGit2SharpStaticRemoteWrapper : IRemoteFunctions
    {
        public bool IsValidName(string name) => Remote.IsValidName(name);
    }
}
