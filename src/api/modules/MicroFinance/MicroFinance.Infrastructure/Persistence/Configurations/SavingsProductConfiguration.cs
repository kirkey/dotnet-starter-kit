namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the SavingsProduct entity.
/// </summary>
internal sealed class SavingsProductConfiguration : IEntityTypeConfiguration<SavingsProduct>
{
    public void Configure(EntityTypeBuilder<SavingsProduct> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(SavingsProduct.CodeMaxLength);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(SavingsProduct.NameMaxLength);

        builder.Property(x => x.Description)
            .HasMaxLength(SavingsProduct.DescriptionMaxLength);

        builder.Property(x => x.CurrencyCode)
            .IsRequired()
            .HasMaxLength(SavingsProduct.CurrencyCodeMaxLength);

        builder.Property(x => x.InterestCalculation)
            .IsRequired()
            .HasMaxLength(SavingsProduct.InterestCalculationMaxLength);

        builder.Property(x => x.InterestPostingFrequency)
            .IsRequired()
            .HasMaxLength(SavingsProduct.InterestPostingFrequencyMaxLength);

        builder.Property(x => x.InterestRate).HasPrecision(5, 2);
        builder.Property(x => x.MinOpeningBalance).HasPrecision(18, 2);
        builder.Property(x => x.MinBalanceForInterest).HasPrecision(18, 2);
        builder.Property(x => x.MinWithdrawalAmount).HasPrecision(18, 2);
        builder.Property(x => x.MaxWithdrawalPerDay).HasPrecision(18, 2);
        builder.Property(x => x.OverdraftLimit).HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("IX_SavingsProducts_Code");

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_SavingsProducts_Name");

        builder.HasIndex(x => x.IsActive)
            .HasDatabaseName("IX_SavingsProducts_IsActive");
    }
}

