using Accounting.Domain.Entities;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class CostCenterConfiguration : IEntityTypeConfiguration<CostCenter>
{
    public void Configure(EntityTypeBuilder<CostCenter> builder)
    {
        builder.ToTable("CostCenters", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Code).IsUnique();

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.CostCenterType)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.ParentCostCenterId);

        builder.Property(x => x.ManagerId);

        builder.Property(x => x.ManagerName)
            .HasMaxLength(256);

        builder.Property(x => x.BudgetAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.ActualAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Location)
            .HasMaxLength(256);

        builder.Property(x => x.StartDate);

        builder.Property(x => x.EndDate);

        builder.Property(x => x.Description)
            .HasMaxLength(2048);

        builder.Property(x => x.Notes)
            .HasMaxLength(2048);
    }
}
