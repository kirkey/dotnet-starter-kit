namespace Store.Infrastructure.Persistence.Configurations;

public class SalesOrderItemConfiguration : IEntityTypeConfiguration<SalesOrderItem>
{
    public void Configure(EntityTypeBuilder<SalesOrderItem> builder)
    {
        builder.HasKey(x => x.Id);

        // Ensure required fields
        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.ShippedQuantity)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.DiscountAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0m);

        builder.Property(x => x.TotalPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.WholesaleTierPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Notes)
            .HasMaxLength(1000)
            .IsUnicode()
            .IsRequired(false);

        builder.HasOne(x => x.SalesOrder)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.GroceryItem)
            .WithMany()
            .HasForeignKey(x => x.GroceryItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // Use the ToTable(RelationalTableBuilder) overload to declare check constraints at table level
        builder.ToTable("SalesOrderItems", "Store", tb =>
        {
            tb.HasCheckConstraint("CK_SalesOrderItems_Quantity_Positive", "[Quantity] > 0");
            tb.HasCheckConstraint("CK_SalesOrderItems_ShippedRange", "[ShippedQuantity] >= 0 AND [ShippedQuantity] <= [Quantity]");
            tb.HasCheckConstraint("CK_SalesOrderItems_DiscountRange", "[DiscountAmount] >= 0 AND [DiscountAmount] <= ([Quantity] * [UnitPrice])");
            tb.HasCheckConstraint("CK_SalesOrderItems_UnitPrice_NonNegative", "[UnitPrice] >= 0");
        });
    }
}
