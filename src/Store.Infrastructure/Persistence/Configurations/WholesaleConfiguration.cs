namespace Store.Infrastructure.Persistence.Configurations;

public class WholesaleConfiguration : IEntityTypeConfiguration<WholesaleContract>
{
    public void Configure(EntityTypeBuilder<WholesaleContract> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ContractNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.ContractNumber)
            .IsUnique();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.MinimumOrderValue)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.VolumeDiscountPercentage)
            .HasColumnType("decimal(5,2)");

        builder.Property(x => x.CreditLimit)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.DeliveryTerms)
            .HasMaxLength(500);

        builder.Property(x => x.ContractTerms)
            .HasMaxLength(5000);

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.HasOne(x => x.Customer)
            .WithMany(x => x.WholesaleContracts)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("WholesaleContracts", "Store");
    }
}
