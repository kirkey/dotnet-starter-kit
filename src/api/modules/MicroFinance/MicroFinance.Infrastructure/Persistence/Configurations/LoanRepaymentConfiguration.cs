namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the LoanRepayment entity.
/// </summary>
internal sealed class LoanRepaymentConfiguration : IEntityTypeConfiguration<LoanRepayment>
{
    public void Configure(EntityTypeBuilder<LoanRepayment> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ReceiptNumber)
            .HasMaxLength(64);

        builder.Property(x => x.PaymentMethod)
            .HasMaxLength(32);

        builder.Property(x => x.TotalAmount).HasPrecision(18, 2);

        // Relationships
        builder.HasOne<Loan>()
            .WithMany()
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.LoanId)
            .HasDatabaseName("IX_LoanRepayments_LoanId");


        builder.HasIndex(x => x.ReceiptNumber)
            .HasDatabaseName("IX_LoanRepayments_ReceiptNumber");
    }
}

