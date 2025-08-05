using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class DepreciationMethodConfiguration : IEntityTypeConfiguration<DepreciationMethod>
{
    public void Configure(EntityTypeBuilder<DepreciationMethod> builder)
    {
        builder.ToTable("DepreciationMethods", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.IsActive)
            .IsRequired();
    }
}
