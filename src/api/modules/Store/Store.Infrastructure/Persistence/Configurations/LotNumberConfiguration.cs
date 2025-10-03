using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the LotNumber entity.
/// </summary>
public class LotNumberConfiguration : IEntityTypeConfiguration<LotNumber>
{
    public void Configure(EntityTypeBuilder<LotNumber> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.LotCode)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<string>();

        // Unique constraint: one lot code per item
        builder.HasIndex(x => new { x.ItemId, x.LotCode })
            .IsUnique();

        // Foreign key relationships
        builder.HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Supplier)
            .WithMany()
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.ToTable("LotNumbers", SchemaNames.Store);
    }
}
