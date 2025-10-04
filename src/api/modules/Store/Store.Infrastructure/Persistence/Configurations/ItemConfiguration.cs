using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Item entity.
/// </summary>
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
            .HasMaxLength(50);

        builder.HasIndex(x => x.Barcode)
            .IsUnique();

        builder.Property(x => x.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(x => x.Cost)
            .HasPrecision(18, 2);

        builder.Property(x => x.UnitOfMeasure)
            .HasMaxLength(20)
            .HasConversion<string>();

        builder.Property(x => x.Weight)
            .HasPrecision(18, 3);

        builder.Property(x => x.WeightUnit)
            .HasMaxLength(10);

        builder.Property(x => x.Length)
            .HasPrecision(18, 3);

        builder.Property(x => x.Width)
            .HasPrecision(18, 3);

        builder.Property(x => x.Height)
            .HasPrecision(18, 3);

        builder.Property(x => x.DimensionUnit)
            .HasMaxLength(10);

        // Foreign key relationships
        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Supplier)
            .WithMany()
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("Items", SchemaNames.Store);
    }
}
