namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the LoanSchedule entity.
/// </summary>
internal sealed class LoanScheduleConfiguration : IEntityTypeConfiguration<LoanSchedule>
{
    public void Configure(EntityTypeBuilder<LoanSchedule> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PrincipalAmount).HasPrecision(18, 2);
        builder.Property(x => x.InterestAmount).HasPrecision(18, 2);
        builder.Property(x => x.PaidAmount).HasPrecision(18, 2);

        // Relationships
        builder.HasOne(x => x.Loan)
            .WithMany(l => l.LoanSchedules)
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(x => new { x.LoanId, x.InstallmentNumber })
            .IsUnique()
            .HasDatabaseName("IX_LoanSchedules_LoanInstallment");

        builder.HasIndex(x => x.LoanId)
            .HasDatabaseName("IX_LoanSchedules_LoanId");

        builder.HasIndex(x => x.DueDate)
            .HasDatabaseName("IX_LoanSchedules_DueDate");

        builder.HasIndex(x => x.IsPaid)
            .HasDatabaseName("IX_LoanSchedules_IsPaid");
    }
}
