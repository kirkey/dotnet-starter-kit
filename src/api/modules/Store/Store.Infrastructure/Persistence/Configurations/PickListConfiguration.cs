using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the PickList entity.
/// </summary>
public class PickListConfiguration : IEntityTypeConfiguration<PickList>
{
    public void Configure(EntityTypeBuilder<PickList> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PickListNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.PickListNumber)
            .IsUnique();

        builder.Property(x => x.PickingType)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<string>();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<string>();

        // Foreign key relationships
        builder.HasOne(x => x.Warehouse)
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        // One-to-many relationship with PickListItems
        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(x => x.PickListId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for foreign keys and query optimization
        builder.HasIndex(x => x.WarehouseId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.PickingType);
        builder.HasIndex(x => x.Priority);

        builder.ToTable("PickLists", SchemaNames.Store);
    }
}
