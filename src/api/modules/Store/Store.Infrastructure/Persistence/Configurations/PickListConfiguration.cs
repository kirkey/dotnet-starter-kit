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
            .WithOne(x => x.PickList)
            .HasForeignKey(x => x.PickListId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("PickLists", SchemaNames.Store);
    }
}
