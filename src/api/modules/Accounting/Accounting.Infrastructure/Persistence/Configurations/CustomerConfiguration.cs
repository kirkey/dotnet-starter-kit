namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for Customer entity.
/// </summary>
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers", SchemaNames.Accounting);
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.CustomerNumber).IsRequired().HasMaxLength(50);
        builder.Property(x => x.CustomerName).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x => x.CustomerType).IsRequired().HasMaxLength(32);
        builder.Property(x => x.BillingAddress).IsRequired().HasMaxLength(500);
        builder.Property(x => x.ShippingAddress).HasMaxLength(500);
        builder.Property(x => x.Email).HasMaxLength(256);
        builder.Property(x => x.Phone).HasMaxLength(50);
        builder.Property(x => x.Fax).HasMaxLength(50);
        builder.Property(x => x.ContactName).HasMaxLength(256);
        builder.Property(x => x.ContactEmail).HasMaxLength(256);
        builder.Property(x => x.ContactPhone).HasMaxLength(50);
        builder.Property(x => x.PaymentTerms).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(32);
        builder.Property(x => x.TaxId).HasMaxLength(50);
        builder.Property(x => x.SalesRepresentative).HasMaxLength(256);
        builder.Property(x => x.Description).HasMaxLength(2048);
        builder.Property(x => x.Notes).HasMaxLength(2048);
        
        builder.Property(x => x.CreditLimit).HasPrecision(18, 2);
        builder.Property(x => x.CurrentBalance).HasPrecision(18, 2);
        builder.Property(x => x.DiscountPercentage).HasPrecision(5, 4);
        builder.Property(x => x.LastPaymentAmount).HasPrecision(18, 2);
        
        builder.HasIndex(x => x.CustomerNumber).IsUnique();
        builder.HasIndex(x => x.CustomerName);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.IsActive);
    }
}

