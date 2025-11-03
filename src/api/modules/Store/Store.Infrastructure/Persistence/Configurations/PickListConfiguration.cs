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

        // Ensure EF uses the backing field for change tracking
        var navigation = builder.Metadata.FindNavigation(nameof(PickList.Items));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        // Indexes for foreign keys and query optimization
        builder.HasIndex(x => x.WarehouseId)
            .HasDatabaseName("IX_PickLists_WarehouseId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_PickLists_Status");

        builder.HasIndex(x => x.PickingType)
            .HasDatabaseName("IX_PickLists_PickingType");

        builder.HasIndex(x => x.Priority)
            .HasDatabaseName("IX_PickLists_Priority");

        builder.HasIndex(x => x.AssignedTo)
            .HasDatabaseName("IX_PickLists_AssignedTo");

        // Composite indexes for common query patterns
        builder.HasIndex(x => new { x.WarehouseId, x.Status })
            .HasDatabaseName("IX_PickLists_Warehouse_Status");

        builder.HasIndex(x => new { x.Status, x.Priority })
            .HasDatabaseName("IX_PickLists_Status_Priority");

        builder.HasIndex(x => new { x.AssignedTo, x.Status })
            .HasDatabaseName("IX_PickLists_AssignedTo_Status");

        builder.ToTable("PickLists", SchemaNames.Store);
    }
}
