namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the CreditBureauInquiry entity.
/// </summary>
internal sealed class CreditBureauInquiryConfiguration : IEntityTypeConfiguration<CreditBureauInquiry>
{
    public void Configure(EntityTypeBuilder<CreditBureauInquiry> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.InquiryNumber)
            .HasMaxLength(CreditBureauInquiry.MaxLengths.InquiryNumber);

        builder.Property(x => x.BureauName)
            .HasMaxLength(CreditBureauInquiry.MaxLengths.BureauName);

        builder.Property(x => x.Purpose)
            .HasMaxLength(CreditBureauInquiry.MaxLengths.Purpose);

        builder.Property(x => x.RequestedBy)
            .HasMaxLength(CreditBureauInquiry.MaxLengths.RequestedBy);

        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(CreditBureauInquiry.MaxLengths.ReferenceNumber);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.InquiryCost)
            .HasPrecision(18, 2);


        // Relationships
        builder.HasOne(x => x.Member)
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Loan)
            .WithMany()
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        // One-to-one relationship with CreditBureauReport
        builder.HasOne(x => x.CreditReport)
            .WithOne(x => x.Inquiry)
            .HasForeignKey<CreditBureauInquiry>(x => x.CreditReportId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.MemberId);
        builder.HasIndex(x => x.LoanId);
        builder.HasIndex(x => x.RequestedByUserId);
        builder.HasIndex(x => x.CreditReportId);
        builder.HasIndex(x => x.Status);
    }
}
