using System;
using System.Data.Common;

namespace Safir.FileManager.Infrastructure
{
    public class FileManagerOptions
    {
        private Lazy<string> _connectionString;

        public FileManagerOptions()
        {
            _connectionString = new Lazy<string>(() => ConnectionStringBuilder.ConnectionString);
        }

        public string ConnectionString {
            get => _connectionString.Value;
            set => _connectionString = new Lazy<string>(() => value);
        }

        public DbConnectionStringBuilder ConnectionStringBuilder { get; set; } = new DbConnectionStringBuilder();
    }
}
