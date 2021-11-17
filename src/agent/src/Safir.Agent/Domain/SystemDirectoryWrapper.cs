using System.Collections.Generic;
using System.IO;

namespace Safir.Agent.Domain
{
    internal sealed class SystemDirectoryWrapper : IDirectory
    {
        public IEnumerable<string> EnumerateFileSystemEntries(string path)
        {
            return Directory.EnumerateFileSystemEntries(path);
        }

        public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
        {
            return Directory.EnumerateFileSystemEntries(path, searchPattern);
        }

        public IEnumerable<string> EnumerateFileSystemEntries(
            string path,
            string searchPattern,
            SearchOption searchOption)
        {
            return Directory.EnumerateFileSystemEntries(path, searchPattern, searchOption);
        }

        public IEnumerable<string> EnumerateFileSystemEntries(
            string path,
            string searchPattern,
            EnumerationOptions enumerationOptions)
        {
            return Directory.EnumerateFileSystemEntries(path, searchPattern, enumerationOptions);
        }

        public bool Exists(string? path)
        {
            return Directory.Exists(path);
        }
    }
}
