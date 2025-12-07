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

        // Relationships
        builder.HasOne<Loan>()
            .WithMany()
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.LoanId)
            .HasDatabaseName("IX_LoanSchedules_LoanId");


        builder.HasIndex(x => new { x.LoanId, x.InstallmentNumber })
            .IsUnique()
            .HasDatabaseName("IX_LoanSchedules_Unique");
    }
}

