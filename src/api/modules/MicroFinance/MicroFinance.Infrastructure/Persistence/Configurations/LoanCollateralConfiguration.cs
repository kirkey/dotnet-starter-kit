namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the LoanCollateral entity.
/// </summary>
internal sealed class LoanCollateralConfiguration : IEntityTypeConfiguration<LoanCollateral>
{
    public void Configure(EntityTypeBuilder<LoanCollateral> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CollateralType)
            .HasMaxLength(64);

        builder.Property(x => x.Description)
            .HasMaxLength(2048);

        builder.Property(x => x.Status)
            .HasMaxLength(32);

        builder.Property(x => x.EstimatedValue).HasPrecision(18, 2);
        builder.Property(x => x.ForcedSaleValue).HasPrecision(18, 2);

        // Relationships
        builder.HasOne(x => x.Loan)
            .WithMany(x => x.LoanCollaterals)
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.LoanId)
            .HasDatabaseName("IX_LoanCollaterals_LoanId");

        builder.HasIndex(x => x.CollateralType)
            .HasDatabaseName("IX_LoanCollaterals_CollateralType");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_LoanCollaterals_Status");
    }
}

