using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Safir.FileManager.Infrastructure.Data
{
    internal class ConfigureDbContextOptions : IConfigureOptions<DbContextOptions<FileContext>>
    {
        private readonly IOptions<FileManagerOptions> _options;

        public ConfigureDbContextOptions(IOptions<FileManagerOptions> options)
        {
            _options = options;
        }

        public void Configure(DbContextOptions<FileContext> options)
        {
            options.
        }
    }
}
