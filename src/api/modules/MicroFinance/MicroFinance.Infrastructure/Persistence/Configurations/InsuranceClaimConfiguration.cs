namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the InsuranceClaim entity.
/// </summary>
internal sealed class InsuranceClaimConfiguration : IEntityTypeConfiguration<InsuranceClaim>
{
    public void Configure(EntityTypeBuilder<InsuranceClaim> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ClaimNumber)
            .HasMaxLength(InsuranceClaim.MaxLengths.ClaimNumber);

        builder.Property(x => x.ClaimType)
            .HasMaxLength(InsuranceClaim.MaxLengths.ClaimType);

        builder.Property(x => x.Description)
            .HasMaxLength(InsuranceClaim.MaxLengths.Description);

        builder.Property(x => x.ClaimAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.ApprovedAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.PaidAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.RejectionReason)
            .HasMaxLength(InsuranceClaim.MaxLengths.RejectionReason);

        builder.Property(x => x.PaymentReference)
            .HasMaxLength(InsuranceClaim.MaxLengths.PaymentReference);

        // Relationships
        builder.HasOne(x => x.InsurancePolicy)
            .WithMany()
            .HasForeignKey(x => x.InsurancePolicyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.InsurancePolicyId);
        builder.HasIndex(x => x.ReviewedByUserId);
        builder.HasIndex(x => x.ApprovedByUserId);
        builder.HasIndex(x => x.Status);
    }
}