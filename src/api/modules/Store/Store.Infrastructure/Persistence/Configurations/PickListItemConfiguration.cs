using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the PickListItem entity.
/// </summary>
public class PickListItemConfiguration : IEntityTypeConfiguration<PickListItem>
{
    public void Configure(EntityTypeBuilder<PickListItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<string>();

        // Foreign key relationships
        builder.HasOne<PickList>()
            .WithMany(p => p.Items)
            .HasForeignKey(x => x.PickListId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Bin)
            .WithMany()
            .HasForeignKey(x => x.BinId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.LotNumber)
            .WithMany()
            .HasForeignKey(x => x.LotNumberId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(x => x.SerialNumber)
            .WithMany()
            .HasForeignKey(x => x.SerialNumberId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        // Indexes for foreign keys and query optimization
        builder.HasIndex(x => x.PickListId)
            .HasDatabaseName("IX_PickListItems_PickListId");

        builder.HasIndex(x => x.ItemId)
            .HasDatabaseName("IX_PickListItems_ItemId");

        builder.HasIndex(x => x.BinId)
            .HasDatabaseName("IX_PickListItems_BinId");

        builder.HasIndex(x => x.LotNumberId)
            .HasDatabaseName("IX_PickListItems_LotNumberId");

        builder.HasIndex(x => x.SerialNumberId)
            .HasDatabaseName("IX_PickListItems_SerialNumberId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_PickListItems_Status");

        // Composite index for picklist+item reporting
        builder.HasIndex(x => new { x.PickListId, x.ItemId })
            .HasDatabaseName("IX_PickListItems_PickList_Item");

        // Composite index for bin location queries
        builder.HasIndex(x => new { x.BinId, x.Status })
            .HasDatabaseName("IX_PickListItems_Bin_Status");

        builder.ToTable("PickListItems", SchemaNames.Store);
    }
}
