using System;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace Safir.FileManager.Infrastructure.Configuration
{
    internal class DefaultDbConnectionConfiguration : IConfigureOptions<FileManagerOptions>
    {
        public void Configure(FileManagerOptions options)
        {
            if (!string.IsNullOrWhiteSpace(options.ConnectionString)) return;

            var builder = new SqliteConnectionStringBuilder();

            var file = Environment.CurrentDirectory + "/safir.sqlite";

            builder.DataSource = file;

            options.ConnectionStringBuilder = builder;
        }
    }
}
