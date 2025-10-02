namespace Accounting.Infrastructure.Persistence.Configurations;

public class TaxCodeConfiguration : IEntityTypeConfiguration<TaxCode>
{
    public void Configure(EntityTypeBuilder<TaxCode> builder)
    {
        builder.ToTable("TaxCodes", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Code).IsUnique();

        builder.Property(x => x.Code)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.TaxType)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.Rate)
            .HasPrecision(10, 6)
            .IsRequired();

        builder.Property(x => x.IsCompound)
            .IsRequired();

        builder.Property(x => x.Jurisdiction)
            .HasMaxLength(256);

        builder.Property(x => x.EffectiveDate)
            .IsRequired();

        builder.Property(x => x.ExpiryDate);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.TaxCollectedAccountId)
            .IsRequired();

        builder.Property(x => x.TaxPaidAccountId);

        builder.Property(x => x.TaxAuthority)
            .HasMaxLength(256);

        builder.Property(x => x.TaxRegistrationNumber)
            .HasMaxLength(100);

        builder.Property(x => x.ReportingCategory)
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(2048);
    }
}
