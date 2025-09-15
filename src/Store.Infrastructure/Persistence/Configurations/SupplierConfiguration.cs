using Shared.Constants;

namespace Store.Infrastructure.Persistence.Configurations;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
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

        builder.Property(x => x.Website)
            .HasMaxLength(255);

        builder.Property(x => x.CreditLimit)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Rating)
            .HasColumnType("decimal(3,2)");

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        // Add table-level constraints to enforce invariants
        builder.ToTable("Suppliers", SchemaNames.Store, tb =>
        {
            // Use PostgreSQL-compatible constraint expressions (no square brackets)
            tb.HasCheckConstraint("CK_Suppliers_CreditLimit_NonNegative", "CreditLimit IS NULL OR CreditLimit >= 0");
            tb.HasCheckConstraint("CK_Suppliers_PaymentTerms_NonNegative", "PaymentTermsDays >= 0");
            tb.HasCheckConstraint("CK_Suppliers_Rating_Range", "Rating >= 0 AND Rating <= 5");
        });
    }
}
