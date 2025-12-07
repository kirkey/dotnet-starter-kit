namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the LoanWriteOff entity.
/// </summary>
internal sealed class LoanWriteOffConfiguration : IEntityTypeConfiguration<LoanWriteOff>
{
    public void Configure(EntityTypeBuilder<LoanWriteOff> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.WriteOffNumber)
            .HasMaxLength(LoanWriteOff.MaxLengths.WriteOffNumber);

        builder.Property(x => x.WriteOffType)
            .HasMaxLength(LoanWriteOff.MaxLengths.WriteOffType);

        builder.Property(x => x.Reason)
            .HasMaxLength(LoanWriteOff.MaxLengths.Reason);

        builder.Property(x => x.PrincipalWriteOff)
            .HasPrecision(18, 2);

        builder.Property(x => x.InterestWriteOff)
            .HasPrecision(18, 2);

        builder.Property(x => x.PenaltiesWriteOff)
            .HasPrecision(18, 2);

        builder.Property(x => x.FeesWriteOff)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalWriteOff)
            .HasPrecision(18, 2);

        builder.Property(x => x.RecoveredAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.ApprovedBy)
            .HasMaxLength(LoanWriteOff.MaxLengths.ApprovedBy);

        builder.Property(x => x.Notes)
            .HasMaxLength(LoanWriteOff.MaxLengths.Notes);

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