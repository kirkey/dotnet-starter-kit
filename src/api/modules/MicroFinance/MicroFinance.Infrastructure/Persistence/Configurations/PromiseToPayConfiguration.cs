namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the PromiseToPay entity.
/// </summary>
internal sealed class PromiseToPayConfiguration : IEntityTypeConfiguration<PromiseToPay>
{
    public void Configure(EntityTypeBuilder<PromiseToPay> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PromisedAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.ActualAmountPaid)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.Notes)
            .HasMaxLength(4096);

        // Relationships
        builder.HasOne<CollectionCase>()
            .WithMany()
            .HasForeignKey(x => x.CollectionCaseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Loan>()
            .WithMany()
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Member>()
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<CollectionAction>()
            .WithMany()
            .HasForeignKey(x => x.CollectionActionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Staff>()
            .WithMany()
            .HasForeignKey(x => x.RecordedById)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.CollectionCaseId)
            .HasDatabaseName("IX_PromiseToPays_CollectionCaseId");

        builder.HasIndex(x => x.LoanId)
            .HasDatabaseName("IX_PromiseToPays_LoanId");

        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_PromiseToPays_MemberId");

        builder.HasIndex(x => x.PromisedPaymentDate)
            .HasDatabaseName("IX_PromiseToPays_PromisedPaymentDate");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_PromiseToPays_Status");
    }
}
