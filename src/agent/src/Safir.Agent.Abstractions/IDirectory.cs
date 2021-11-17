using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;

namespace Safir.Agent
{
    [PublicAPI]
    public interface IDirectory
    {
        IEnumerable<string> EnumerateFileSystemEntries(string path);

        IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern);

        IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption);

        IEnumerable<string> EnumerateFileSystemEntries(
            string path,
            string searchPattern,
            EnumerationOptions enumerationOptions);

        bool Exists(string? path);
    }
}
