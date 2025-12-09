namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the InterestRateChange entity.
/// </summary>
internal sealed class InterestRateChangeConfiguration : IEntityTypeConfiguration<InterestRateChange>
{
    public void Configure(EntityTypeBuilder<InterestRateChange> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Reference)
            .HasMaxLength(InterestRateChange.ReferenceMaxLength);

        builder.Property(x => x.ChangeReason)
            .HasMaxLength(InterestRateChange.ChangeReasonMaxLength);

        builder.Property(x => x.ChangeType)
            .HasMaxLength(InterestRateChange.ChangeTypeMaxLength);

        builder.Property(x => x.ApprovedBy)
            .HasMaxLength(InterestRateChange.ApprovedByMaxLength);

        builder.Property(x => x.Status)
            .HasMaxLength(InterestRateChange.StatusMaxLength);

        builder.Property(x => x.RejectionReason)
            .HasMaxLength(InterestRateChange.RejectionReasonMaxLength);

        builder.Property(x => x.Notes)
            .HasMaxLength(InterestRateChange.NotesMaxLength);

        builder.Property(x => x.PreviousRate)
            .HasPrecision(8, 4);

        builder.Property(x => x.NewRate)
            .HasPrecision(8, 4);

        // Relationships
        builder.HasOne(x => x.Loan)
            .WithMany(x => x.InterestRateChanges)
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.LoanId)
            .HasDatabaseName("IX_InterestRateChanges_LoanId");

        builder.HasIndex(x => x.EffectiveDate)
            .HasDatabaseName("IX_InterestRateChanges_EffectiveDate");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_InterestRateChanges_Status");
    }
}
