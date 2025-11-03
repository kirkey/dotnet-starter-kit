using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

public class CycleCountItemConfiguration : IEntityTypeConfiguration<CycleCountItem>
{
    public void Configure(EntityTypeBuilder<CycleCountItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.Property(x => x.CountedBy)
            .HasMaxLength(100);

        builder.Property(x => x.RecountReason)
            .HasMaxLength(500);

        builder.HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for query optimization
        builder.HasIndex(x => x.CycleCountId)
            .HasDatabaseName("IX_CycleCountItems_CycleCountId");

        builder.HasIndex(x => x.ItemId)
            .HasDatabaseName("IX_CycleCountItems_ItemId");

        // Ensure an item can only appear once per cycle count
        builder.HasIndex(x => new { x.CycleCountId, x.ItemId })
            .IsUnique()
            .HasDatabaseName("IX_CycleCountItems_CycleCount_Item");

        builder.ToTable("CycleCountItems", SchemaNames.Store);
    }
}
