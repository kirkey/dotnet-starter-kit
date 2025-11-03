using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

public class CycleCountConfiguration : IEntityTypeConfiguration<CycleCount>
{
    public void Configure(EntityTypeBuilder<CycleCount> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CountNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.CountNumber)
            .IsUnique();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.CountType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.CounterName)
            .HasMaxLength(100);

        builder.Property(x => x.SupervisorName)
            .HasMaxLength(100);

        builder.Property(x => x.AccuracyPercentage)
            .HasColumnType("decimal(5,2)");

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.HasOne(x => x.Warehouse)
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.WarehouseLocation)
            .WithMany()
            .HasForeignKey(x => x.WarehouseLocationId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure Items collection with private backing field for proper encapsulation
        // This matches the Budget/BudgetDetails pattern for consistency
        builder.HasMany(x => x.Items)
            .WithOne(x => x.CycleCount)
            .HasForeignKey(x => x.CycleCountId)
            .OnDelete(DeleteBehavior.Cascade);

        // Map the private backing field for the Items collection
        builder.Metadata
            .FindNavigation(nameof(CycleCount.Items))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        // Indexes for foreign keys and query optimization
        builder.HasIndex(x => x.WarehouseId)
            .HasDatabaseName("IX_CycleCounts_WarehouseId");

        builder.HasIndex(x => x.WarehouseLocationId)
            .HasDatabaseName("IX_CycleCounts_WarehouseLocationId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_CycleCounts_Status");

        builder.HasIndex(x => x.CountType)
            .HasDatabaseName("IX_CycleCounts_CountType");

        builder.HasIndex(x => x.ScheduledDate)
            .HasDatabaseName("IX_CycleCounts_ScheduledDate");

        // Composite indexes for common query patterns
        builder.HasIndex(x => new { x.WarehouseId, x.ScheduledDate })
            .HasDatabaseName("IX_CycleCounts_Warehouse_ScheduledDate");

        builder.HasIndex(x => new { x.Status, x.ScheduledDate })
            .HasDatabaseName("IX_CycleCounts_Status_ScheduledDate");

        builder.HasIndex(x => new { x.WarehouseId, x.Status })
            .HasDatabaseName("IX_CycleCounts_Warehouse_Status");

        builder.ToTable("CycleCounts", SchemaNames.Store);
    }
}
