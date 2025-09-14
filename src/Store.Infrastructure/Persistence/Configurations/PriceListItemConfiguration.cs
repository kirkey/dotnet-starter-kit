using Shared.Constants;

namespace Store.Infrastructure.Persistence.Configurations;

public class PriceListItemConfiguration : IEntityTypeConfiguration<PriceListItem>
{
    public void Configure(EntityTypeBuilder<PriceListItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Price)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.DiscountPercentage)
            .HasColumnType("decimal(5,2)");

        builder.Property(x => x.MinimumQuantity)
            .HasColumnType("decimal(18,3)");

        builder.Property(x => x.MaximumQuantity)
            .HasColumnType("decimal(18,3)");

        builder.HasOne(x => x.PriceList)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.PriceListId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.GroceryItem)
            .WithMany()
            .HasForeignKey(x => x.GroceryItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("PriceListItems", SchemaNames.Store);
    }
}
