using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Sku)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Sku)
            .IsUnique();

        builder.Property(x => x.Barcode)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Barcode)
            .IsUnique();

        builder.Property(x => x.Price)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Cost)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Weight)
            .HasColumnType("decimal(18,3)");

        builder.Property(x => x.WeightUnit)
            .HasMaxLength(20);

        builder.Property(x => x.Brand)
            .HasMaxLength(100);

        builder.Property(x => x.Manufacturer)
            .HasMaxLength(100);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Supplier)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.WarehouseLocation)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.WarehouseLocationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.ToTable("Items", SchemaNames.Store);
    }
}
