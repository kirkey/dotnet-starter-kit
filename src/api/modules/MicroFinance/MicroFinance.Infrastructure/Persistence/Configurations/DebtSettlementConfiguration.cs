namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the DebtSettlement entity.
/// </summary>
internal sealed class DebtSettlementConfiguration : IEntityTypeConfiguration<DebtSettlement>
{
    public void Configure(EntityTypeBuilder<DebtSettlement> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.OriginalOutstanding)
            .HasPrecision(18, 2);

        builder.Property(x => x.SettlementAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.DiscountAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.DiscountPercentage)
            .HasPrecision(18, 2);

        builder.Property(x => x.AmountPaid)
            .HasPrecision(18, 2);

        builder.Property(x => x.RemainingBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.InstallmentAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(64);

        builder.Property(x => x.SettlementType)
            .HasMaxLength(32);

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

        builder.HasOne<Staff>()
            .WithMany()
            .HasForeignKey(x => x.ProposedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Staff>()
            .WithMany()
            .HasForeignKey(x => x.ApprovedById)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(x => x.ReferenceNumber)
            .IsUnique()
            .HasDatabaseName("IX_DebtSettlements_ReferenceNumber");

        builder.HasIndex(x => x.CollectionCaseId)
            .HasDatabaseName("IX_DebtSettlements_CollectionCaseId");

        builder.HasIndex(x => x.LoanId)
            .HasDatabaseName("IX_DebtSettlements_LoanId");

        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_DebtSettlements_MemberId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_DebtSettlements_Status");
    }
}
