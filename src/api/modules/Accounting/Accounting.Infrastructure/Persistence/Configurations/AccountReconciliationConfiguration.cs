namespace Accounting.Infrastructure.Persistence.Configurations;

public class AccountReconciliationConfiguration : IEntityTypeConfiguration<AccountReconciliation>
{
    public void Configure(EntityTypeBuilder<AccountReconciliation> builder)
    {
        builder.ToTable("AccountReconciliations", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.GeneralLedgerAccountId)
            .IsRequired();

        builder.Property(x => x.AccountingPeriodId)
            .IsRequired();

        builder.Property(x => x.GlBalance)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.SubsidiaryLedgerBalance)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Variance)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.ReconciliationStatus)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.ReconciliationDate)
            .IsRequired();

        builder.Property(x => x.VarianceExplanation)
            .HasMaxLength(2000);

        builder.Property(x => x.SubsidiaryLedgerSource)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.LineItemCount)
            .IsRequired();

        builder.Property(x => x.AdjustingEntriesRecorded)
            .IsRequired();

        // Store notes as JSON
        builder.Property(x => x.ReconciliationNotes)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>())
            .HasColumnType("jsonb");

        // Indexes
        builder.HasIndex(x => x.GeneralLedgerAccountId);
        builder.HasIndex(x => x.AccountingPeriodId);
        builder.HasIndex(x => x.ReconciliationDate);
        builder.HasIndex(x => x.ReconciliationStatus);
        builder.HasIndex(x => new { x.GeneralLedgerAccountId, x.AccountingPeriodId }).IsUnique();
    }
}
