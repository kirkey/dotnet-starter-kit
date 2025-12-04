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
            .IsRequired()
            .HasMaxLength(LoanCollateral.CollateralTypeMaxLength);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(LoanCollateral.DescriptionMaxLength);

        builder.Property(x => x.Location)
            .HasMaxLength(LoanCollateral.LocationMaxLength);

        builder.Property(x => x.DocumentReference)
            .HasMaxLength(LoanCollateral.DocumentReferenceMaxLength);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(LoanCollateral.StatusMaxLength);

        builder.Property(x => x.Notes)
            .HasMaxLength(LoanCollateral.NotesMaxLength);

        builder.Property(x => x.EstimatedValue).HasPrecision(18, 2);
        builder.Property(x => x.ForcedSaleValue).HasPrecision(18, 2);

        // Relationships
        builder.HasOne(x => x.Loan)
            .WithMany()
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.LoanId)
            .HasDatabaseName("IX_LoanCollaterals_LoanId");

        builder.HasIndex(x => x.CollateralType)
            .HasDatabaseName("IX_LoanCollaterals_CollateralType");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_LoanCollaterals_Status");

        builder.HasIndex(x => x.ValuationDate)
            .HasDatabaseName("IX_LoanCollaterals_ValuationDate");
    }
}
