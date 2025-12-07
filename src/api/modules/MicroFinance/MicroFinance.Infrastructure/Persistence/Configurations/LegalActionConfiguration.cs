namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the LegalAction entity.
/// </summary>
internal sealed class LegalActionConfiguration : IEntityTypeConfiguration<LegalAction>
{
    public void Configure(EntityTypeBuilder<LegalAction> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.ClaimAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.JudgmentAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.AmountRecovered)
            .HasPrecision(18, 2);

        builder.Property(x => x.LegalCosts)
            .HasPrecision(18, 2);

        builder.Property(x => x.CourtFees)
            .HasPrecision(18, 2);

        builder.Property(x => x.CaseReference)
            .HasMaxLength(64);

        builder.Property(x => x.ActionType)
            .HasMaxLength(64);

        builder.Property(x => x.CourtName)
            .HasMaxLength(256);

        builder.Property(x => x.LawyerName)
            .HasMaxLength(256);

        builder.Property(x => x.Notes)
            .HasMaxLength(4096);

        // Relationships
        builder.HasOne(x => x.CollectionCase)
            .WithMany()
            .HasForeignKey(x => x.CollectionCaseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Loan)
            .WithMany()
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Member)
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.CaseReference)
            .HasDatabaseName("IX_LegalActions_CaseReference");

        builder.HasIndex(x => x.CollectionCaseId)
            .HasDatabaseName("IX_LegalActions_CollectionCaseId");

        builder.HasIndex(x => x.LoanId)
            .HasDatabaseName("IX_LegalActions_LoanId");

        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_LegalActions_MemberId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_LegalActions_Status");
    }
}
