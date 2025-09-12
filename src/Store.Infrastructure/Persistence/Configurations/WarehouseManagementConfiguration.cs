namespace Store.Infrastructure.Persistence.Configurations;

public class StockAdjustmentConfiguration : IEntityTypeConfiguration<StockAdjustment>
{
    public void Configure(EntityTypeBuilder<StockAdjustment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AdjustmentNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.AdjustmentNumber)
            .IsUnique();

        builder.Property(x => x.AdjustmentType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Reason)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.UnitCost)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TotalCostImpact)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Reference)
            .HasMaxLength(100);

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.Property(x => x.AdjustedBy)
            .HasMaxLength(100);

        builder.Property(x => x.ApprovedBy)
            .HasMaxLength(100);

        builder.Property(x => x.BatchNumber)
            .HasMaxLength(50);

        builder.HasOne(x => x.GroceryItem)
            .WithMany()
            .HasForeignKey(x => x.GroceryItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Warehouse)
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.WarehouseLocation)
            .WithMany()
            .HasForeignKey(x => x.WarehouseLocationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.ToTable("StockAdjustments", "Store");
    }
}

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

        builder.ToTable("CycleCounts", "Store");
    }
}

public class CycleCountItemConfiguration : IEntityTypeConfiguration<CycleCountItem>
{
    public void Configure(EntityTypeBuilder<CycleCountItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.Property(x => x.CountedBy)
            .HasMaxLength(100);

        builder.Property(x => x.RecountReason)
            .HasMaxLength(500);

        builder.HasOne(x => x.CycleCount)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.CycleCountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.GroceryItem)
            .WithMany()
            .HasForeignKey(x => x.GroceryItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("CycleCountItems", "Store");
    }
}
