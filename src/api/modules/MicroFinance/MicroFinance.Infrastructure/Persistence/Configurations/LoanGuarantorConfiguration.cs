namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the LoanGuarantor entity.
/// </summary>
internal sealed class LoanGuarantorConfiguration : IEntityTypeConfiguration<LoanGuarantor>
{
    public void Configure(EntityTypeBuilder<LoanGuarantor> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(32);
        
        // Relationships
        builder.HasOne(x => x.Loan)
            .WithMany(x => x.LoanGuarantors)
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.GuarantorMember)
            .WithMany()
            .HasForeignKey(x => x.GuarantorMemberId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.LoanId)
            .HasDatabaseName("IX_LoanGuarantors_LoanId");

        builder.HasIndex(x => x.GuarantorMemberId)
            .HasDatabaseName("IX_LoanGuarantors_GuarantorMemberId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_LoanGuarantors_Status");
    }
}

