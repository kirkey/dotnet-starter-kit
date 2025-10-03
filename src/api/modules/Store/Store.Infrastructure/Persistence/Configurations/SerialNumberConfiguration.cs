using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the SerialNumber entity.
/// </summary>
public class SerialNumberConfiguration : IEntityTypeConfiguration<SerialNumber>
{
    public void Configure(EntityTypeBuilder<SerialNumber> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.SerialNumberValue)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.SerialNumberValue)
            .IsUnique();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<string>();

        // Foreign key relationships
        builder.HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Warehouse)
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(x => x.WarehouseLocation)
            .WithMany()
            .HasForeignKey(x => x.WarehouseLocationId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(x => x.Bin)
            .WithMany()
            .HasForeignKey(x => x.BinId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(x => x.LotNumber)
            .WithMany()
            .HasForeignKey(x => x.LotNumberId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.ToTable("SerialNumbers", SchemaNames.Store);
    }
}
