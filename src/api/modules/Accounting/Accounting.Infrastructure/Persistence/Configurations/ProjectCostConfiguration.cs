using Accounting.Domain.Entities;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class ProjectCostEntryConfiguration : IEntityTypeConfiguration<ProjectCostEntry>
{
    public void Configure(EntityTypeBuilder<ProjectCostEntry> builder)
    {
        builder.ToTable("ProjectCostEntries", schema: SchemaNames.Accounting);
        builder.HasKey(pc => pc.Id);

        builder.Property(pc => pc.ProjectId).IsRequired();
        builder.Property(pc => pc.EntryDate).IsRequired();
        builder.Property(pc => pc.Description).HasMaxLength(512).IsRequired();
        builder.Property(pc => pc.Amount).HasPrecision(18, 2).IsRequired();
        builder.Property(pc => pc.Category).HasMaxLength(100);
    }
}
