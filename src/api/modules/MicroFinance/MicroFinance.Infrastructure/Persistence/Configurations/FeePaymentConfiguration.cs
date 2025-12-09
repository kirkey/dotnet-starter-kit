namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the FeePayment entity.
/// </summary>
internal sealed class FeePaymentConfiguration : IEntityTypeConfiguration<FeePayment>
{
    public void Configure(EntityTypeBuilder<FeePayment> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Reference)
            .HasMaxLength(FeePayment.ReferenceMaxLength);

        builder.Property(x => x.PaymentMethod)
            .HasMaxLength(FeePayment.PaymentMethodMaxLength);

        builder.Property(x => x.PaymentSource)
            .HasMaxLength(FeePayment.PaymentSourceMaxLength);

        builder.Property(x => x.Status)
            .HasMaxLength(FeePayment.StatusMaxLength);

        builder.Property(x => x.ReversalReason)
            .HasMaxLength(FeePayment.ReversalReasonMaxLength);

        builder.Property(x => x.Notes)
            .HasMaxLength(FeePayment.NotesMaxLength);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        // Relationships
        builder.HasOne(x => x.FeeCharge)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.FeeChargeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.FeeChargeId)
            .HasDatabaseName("IX_FeePayments_FeeChargeId");

        builder.HasIndex(x => x.PaymentDate)
            .HasDatabaseName("IX_FeePayments_PaymentDate");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_FeePayments_Status");
    }
}
