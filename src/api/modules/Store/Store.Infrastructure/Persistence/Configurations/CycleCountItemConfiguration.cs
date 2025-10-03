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

        builder.HasOne(x => x.CycleCount)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.CycleCountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("CycleCountItems", SchemaNames.Store);
    }
}
