using System.Collections.Generic;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace Cli.Services.Installers.Vcs
{
    public interface IRepositoryFunctions
    {
        string Clone(string sourceUrl, string workdirPath, CloneOptions? options = null);
        
        string? Discover(string startingPath);
                
        bool IsValid(string path);

        string Init(string path, bool isBare = false);

        string Init(string workingDirectoryPath, string gitDirectoryPath);

        IEnumerable<Reference> ListRemoteReferences(string url, CredentialsHandler? credentialsProvider = null);
    }
}
