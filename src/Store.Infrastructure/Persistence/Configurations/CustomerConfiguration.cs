using Shared.Constants;

namespace Store.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.CustomerType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.ContactPerson)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.State)
            .HasMaxLength(100);

        builder.Property(x => x.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.PostalCode)
            .HasMaxLength(20);

        builder.Property(x => x.CreditLimit)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.CurrentBalance)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.DiscountPercentage)
            .HasColumnType("decimal(5,2)");

        builder.Property(x => x.LifetimeValue)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TaxNumber)
            .HasMaxLength(50);

        builder.Property(x => x.BusinessLicense)
            .HasMaxLength(100);

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.ToTable("Customers", SchemaNames.Store);
    }
}
