using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Safir.Manager.Domain;

namespace Safir.Manager.Data.Configuration.Postgres
{
    internal sealed class FileCreatedConfiguration : IEntityTypeConfiguration<FileCreated>
    {
        public void Configure(EntityTypeBuilder<FileCreated> builder)
        {
            builder.Property(x => x.Id).UseIdentityAlwaysColumn();
            builder.Property(x => x.Created).ValueGeneratedOnAdd();
            builder.Property(x => x.Updated).ValueGeneratedOnAddOrUpdate();
        }
    }
}
