using System.Collections.Generic;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace Cli.Internal.Wrappers.Git
{
    public class LibGit2SharpStaticRepositoryWrapper : IRepositoryFunctions
    {
        public string Clone(string sourceUrl, string workdirPath, CloneOptions? options = null)
            => Repository.Clone(sourceUrl, workdirPath, options);

        public string? Discover(string startingPath) => Repository.Discover(startingPath);

        public bool IsValid(string path) => Repository.IsValid(path);

        public string Init(string path, bool isBare = false) => Repository.Init(path, isBare);

        public string Init(string workingDirectoryPath, string gitDirectoryPath)
            => Repository.Init(workingDirectoryPath, gitDirectoryPath);

        public IEnumerable<Reference> ListRemoteReferences(string url, CredentialsHandler? credentialsProvider = null)
            => Repository.ListRemoteReferences(url, credentialsProvider);
    }
}
