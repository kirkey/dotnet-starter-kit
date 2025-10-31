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
        builder.HasIndex(x => x.WarehouseId);
        builder.HasIndex(x => x.WarehouseLocationId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.CountType);
        builder.HasIndex(x => x.ScheduledDate);

        builder.ToTable("CycleCounts", SchemaNames.Store);
    }
}
