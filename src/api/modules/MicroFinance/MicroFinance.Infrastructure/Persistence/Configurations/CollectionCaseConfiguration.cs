namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the CollectionCase entity.
/// </summary>
internal sealed class CollectionCaseConfiguration : IEntityTypeConfiguration<CollectionCase>
{
    public void Configure(EntityTypeBuilder<CollectionCase> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.AmountOverdue)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalOutstanding)
            .HasPrecision(18, 2);

        builder.Property(x => x.AmountRecovered)
            .HasPrecision(18, 2);

        builder.Property(x => x.CaseNumber)
            .HasMaxLength(64);

        builder.Property(x => x.Priority)
            .HasMaxLength(32);

        builder.Property(x => x.Notes)
            .HasMaxLength(4096);

        // Relationships
        builder.HasOne(x => x.Loan)
            .WithMany()
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Member)
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Staff>()
            .WithMany()
            .HasForeignKey(x => x.AssignedCollectorId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(x => x.CaseNumber)
            .IsUnique()
            .HasDatabaseName("IX_CollectionCases_CaseNumber");

        builder.HasIndex(x => x.LoanId)
            .HasDatabaseName("IX_CollectionCases_LoanId");

        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_CollectionCases_MemberId");

        builder.HasIndex(x => x.AssignedCollectorId)
            .HasDatabaseName("IX_CollectionCases_AssignedCollectorId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_CollectionCases_Status");
    }
}
