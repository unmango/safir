using System;
using System.IO;
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

            var file = Environment.CurrentDirectory + "safir.sqlite";

            if (!File.Exists(file)) File.Create(file).Close();

            builder.DataSource = file;

            options.ConnectionStringBuilder = builder;
        }
    }
}
