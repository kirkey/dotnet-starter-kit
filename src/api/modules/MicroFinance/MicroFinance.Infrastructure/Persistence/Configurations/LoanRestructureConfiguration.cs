namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the LoanRestructure entity.
/// </summary>
internal sealed class LoanRestructureConfiguration : IEntityTypeConfiguration<LoanRestructure>
{
    public void Configure(EntityTypeBuilder<LoanRestructure> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RestructureNumber)
            .HasMaxLength(LoanRestructure.MaxLengths.RestructureNumber);

        builder.Property(x => x.RestructureType)
            .HasMaxLength(LoanRestructure.MaxLengths.RestructureType);

        builder.Property(x => x.Reason)
            .HasMaxLength(LoanRestructure.MaxLengths.Reason);

        builder.Property(x => x.OriginalPrincipal)
            .HasPrecision(18, 2);

        builder.Property(x => x.OriginalInterestRate)
            .HasPrecision(18, 2);

        builder.Property(x => x.OriginalInstallmentAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.NewPrincipal)
            .HasPrecision(18, 2);

        builder.Property(x => x.NewInterestRate)
            .HasPrecision(18, 2);

        builder.Property(x => x.NewInstallmentAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.WaivedAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.RestructureFee)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.ApprovedBy)
            .HasMaxLength(LoanRestructure.MaxLengths.ApprovedBy);

        builder.Property(x => x.Notes)
            .HasMaxLength(LoanRestructure.MaxLengths.Notes);

        // Relationships
        builder.HasOne(x => x.Loan)
            .WithMany()
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.LoanId);
        builder.HasIndex(x => x.ApprovedByUserId);
        builder.HasIndex(x => x.Status);
    }
}