namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the LoanProduct entity.
/// </summary>
internal sealed class LoanProductConfiguration : IEntityTypeConfiguration<LoanProduct>
{
    public void Configure(EntityTypeBuilder<LoanProduct> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(LoanProduct.CodeMaxLength);

        builder.Property(x => x.Name)
            .HasMaxLength(LoanProduct.NameMaxLength);

        builder.Property(x => x.Description)
            .HasMaxLength(LoanProduct.DescriptionMaxLength);

        builder.Property(x => x.RepaymentFrequency)
            .HasMaxLength(LoanProduct.RepaymentFrequencyMaxLength);

        builder.Property(x => x.InterestMethod)
            .HasMaxLength(LoanProduct.InterestMethodMaxLength);

        builder.Property(x => x.MinLoanAmount).HasPrecision(18, 2);
        builder.Property(x => x.MaxLoanAmount).HasPrecision(18, 2);
        builder.Property(x => x.InterestRate).HasPrecision(8, 4);

        // Indexes
        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("IX_LoanProducts_Code");

        builder.HasIndex(x => x.IsActive)
            .HasDatabaseName("IX_LoanProducts_IsActive");
    }
}

