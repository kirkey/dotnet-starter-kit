using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the InventoryReservation entity.
/// </summary>
public class InventoryReservationConfiguration : IEntityTypeConfiguration<InventoryReservation>
{
    public void Configure(EntityTypeBuilder<InventoryReservation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ReservationNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.ReservationNumber)
            .IsUnique();

        builder.Property(x => x.ReservationType)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<string>();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<string>();

        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(100);

        // Foreign key relationships
        builder.HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Warehouse)
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.LotNumber)
            .WithMany()
            .HasForeignKey(x => x.LotNumberId)
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

        // Indexes for foreign keys and query optimization
        builder.HasIndex(x => x.ItemId);
        builder.HasIndex(x => x.WarehouseId);
        builder.HasIndex(x => x.LotNumberId);
        builder.HasIndex(x => x.WarehouseLocationId);
        builder.HasIndex(x => x.BinId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.ReservationType);
        builder.HasIndex(x => x.ReservationDate);
        builder.HasIndex(x => x.ExpirationDate);

        builder.ToTable("InventoryReservations", SchemaNames.Store);
    }
}
