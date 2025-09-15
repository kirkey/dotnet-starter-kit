using Shared.Constants;

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
            .HasMaxLength(200);

        builder.Property(x => x.UnitCost)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0m);

        builder.Property(x => x.TotalCostImpact)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0m);

        builder.Property(x => x.Reference)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.Notes)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(x => x.AdjustedBy)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.ApprovedBy)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.BatchNumber)
            .HasMaxLength(50)
            .IsRequired(false);

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

        // Table-level constraints to enforce invariants
        builder.ToTable("StockAdjustments", SchemaNames.Store, tb =>
        {
            tb.HasCheckConstraint("CK_StockAdjustments_QuantityBefore_NonNegative", "QuantityBefore >= 0");
            tb.HasCheckConstraint("CK_StockAdjustments_AdjustmentQuantity_Positive", "AdjustmentQuantity > 0");
            tb.HasCheckConstraint("CK_StockAdjustments_QuantityAfter_NonNegative", "QuantityAfter >= 0");
            tb.HasCheckConstraint("CK_StockAdjustments_UnitCost_NonNegative", "UnitCost >= 0");
        });
    }
}
