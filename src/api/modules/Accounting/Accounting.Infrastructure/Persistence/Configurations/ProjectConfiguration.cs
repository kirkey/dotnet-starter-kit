using Accounting.Domain.Entities;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StartDate)
            .IsRequired();

        builder.Property(x => x.EndDate);

        builder.Property(x => x.BudgetedAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(x => x.ClientName)
            .HasMaxLength(256);

        builder.Property(x => x.ProjectManager)
            .HasMaxLength(256);

        builder.Property(x => x.Department)
            .HasMaxLength(100);

        builder.Property(x => x.ActualCost)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.ActualRevenue)
            .HasPrecision(18, 2)
            .IsRequired();

        // One-to-many relationship to ProjectCost entries
        builder
            .HasMany(p => p.CostingEntries)
            .WithOne()
            .HasForeignKey(pc => pc.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
