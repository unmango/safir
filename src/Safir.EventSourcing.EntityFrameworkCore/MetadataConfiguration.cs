using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Safir.EventSourcing.EntityFrameworkCore
{
    public class MetadataConfiguration : IEntityTypeConfiguration<Metadata>
    {
        public void Configure(EntityTypeBuilder<Metadata> builder)
        {
            builder.Property(x => x.CausationId).IsRequired();
            builder.Property(x => x.CorrelationId).IsRequired();
        }
    }
}
