using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the ItemSupplier entity.
/// </summary>
public class ItemSupplierConfiguration : IEntityTypeConfiguration<ItemSupplier>
{
    public void Configure(EntityTypeBuilder<ItemSupplier> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.SupplierPartNumber)
            .HasMaxLength(100);

        builder.Property(x => x.UnitCost)
            .HasPrecision(18, 2);

        // Unique constraint: one relationship per item-supplier combination
        builder.HasIndex(x => new { x.ItemId, x.SupplierId })
            .IsUnique();

        // Foreign key relationships
        builder.HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Supplier)
            .WithMany()
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("ItemSuppliers", SchemaNames.Store);
    }
}
