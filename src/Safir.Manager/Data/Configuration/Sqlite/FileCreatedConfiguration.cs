using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Safir.Manager.Domain;

namespace Safir.Manager.Data.Configuration.Sqlite
{
    internal sealed class FileCreatedConfiguration : IEntityTypeConfiguration<FileCreated>
    {
        public void Configure(EntityTypeBuilder<FileCreated> builder)
        {
            builder.Property(x => x.Id);
            builder.Property(x => x.Created);
            builder.Property(x => x.Updated);
        }
    }
}
