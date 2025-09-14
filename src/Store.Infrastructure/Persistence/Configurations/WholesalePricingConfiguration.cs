using Shared.Constants;

namespace Store.Infrastructure.Persistence.Configurations;

public class WholesalePricingConfiguration : IEntityTypeConfiguration<WholesalePricing>
{
    public void Configure(EntityTypeBuilder<WholesalePricing> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TierPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.DiscountPercentage)
            .HasColumnType("decimal(5,2)");

        builder.Property(x => x.Notes)
            .HasMaxLength(2048);

        builder.HasOne(x => x.WholesaleContract)
            .WithMany(x => x.WholesalePricings)
            .HasForeignKey(x => x.WholesaleContractId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.GroceryItem)
            .WithMany()
            .HasForeignKey(x => x.GroceryItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("WholesalePricings", SchemaNames.Store);
    }
}
