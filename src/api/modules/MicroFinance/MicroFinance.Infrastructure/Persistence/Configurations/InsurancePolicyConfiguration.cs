namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the InsurancePolicy entity.
/// </summary>
internal sealed class InsurancePolicyConfiguration : IEntityTypeConfiguration<InsurancePolicy>
{
    public void Configure(EntityTypeBuilder<InsurancePolicy> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PolicyNumber)
            .HasMaxLength(InsurancePolicy.MaxLengths.PolicyNumber);

        builder.Property(x => x.CoverageAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.PremiumAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalPremiumPaid)
            .HasPrecision(18, 2);

        builder.Property(x => x.BeneficiaryName)
            .HasMaxLength(InsurancePolicy.MaxLengths.BeneficiaryName);

        builder.Property(x => x.BeneficiaryRelation)
            .HasMaxLength(InsurancePolicy.MaxLengths.BeneficiaryRelation);

        builder.Property(x => x.BeneficiaryContact)
            .HasMaxLength(InsurancePolicy.MaxLengths.BeneficiaryContact);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        // Relationships
        builder.HasOne(x => x.InsuranceProduct)
            .WithMany(x => x.Policies)
            .HasForeignKey(x => x.InsuranceProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Member)
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Loan)
            .WithMany()
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.InsuranceProductId);
        builder.HasIndex(x => x.MemberId);
        builder.HasIndex(x => x.LoanId);
        builder.HasIndex(x => x.Status);
    }
}
