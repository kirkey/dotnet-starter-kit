using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Bin entity.
/// </summary>
public class BinConfiguration : IEntityTypeConfiguration<Bin>
{
    public void Configure(EntityTypeBuilder<Bin> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.BinType)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<string>();

        builder.Property(x => x.Capacity)
            .HasPrecision(18, 2);

        builder.Property(x => x.CurrentUtilization)
            .HasPrecision(18, 2);

        // Unique constraint: one bin code per warehouse location
        builder.HasIndex(x => new { x.WarehouseLocationId, x.Code })
            .IsUnique();

        // Foreign key relationship
        builder.HasOne(x => x.WarehouseLocation)
            .WithMany()
            .HasForeignKey(x => x.WarehouseLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("Bins", SchemaNames.Store);
    }
}
