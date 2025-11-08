namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for GeneralLedger entity.
/// Configures database mapping, indexes, and constraints for GL entries.
/// </summary>
public class GeneralLedgerConfiguration : IEntityTypeConfiguration<GeneralLedger>
{
    public void Configure(EntityTypeBuilder<GeneralLedger> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(nameof(GeneralLedger), SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.TransactionDate)
            .IsRequired()
            .HasComment("Date of the transaction");

        builder.Property(x => x.AccountCode)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Chart of account code");

        builder.Property(x => x.Debit)
            .IsRequired()
            .HasPrecision(16, 2)
            .HasComment("Debit amount");

        builder.Property(x => x.Credit)
            .IsRequired()
            .HasPrecision(16, 2)
            .HasComment("Credit amount");

        builder.Property(x => x.Memo)
            .HasMaxLength(500)
            .HasComment("Transaction memo or description");

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .HasComment("Transaction description");

        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(100)
            .HasComment("Reference number or document number");

        builder.Property(x => x.Source)
            .HasMaxLength(100)
            .HasComment("Source of the transaction (Invoice, Bill, JournalEntry, etc.)");

        builder.Property(x => x.SourceId)
            .HasComment("ID of the source document");

        builder.Property(x => x.UsoaClass)
            .HasMaxLength(100)
            .HasComment("USOA classification for regulatory reporting");

        builder.Property(x => x.PeriodId)
            .HasComment("Accounting period ID");

        builder.Property(x => x.IsPosted)
            .IsRequired()
            .HasComment("Whether the entry has been posted");

        builder.Property(x => x.PostedDate)
            .HasComment("Date when the entry was posted");

        builder.Property(x => x.PostedBy)
            .HasMaxLength(256)
            .HasComment("User who posted the entry");

        builder.Property(x => x.Notes)
            .HasMaxLength(2000)
            .HasComment("Additional notes");

        builder.Property(x => x.PostedBy)
            .HasMaxLength(256)
            .HasComment("User who posted the entry");

        builder.Property(x => x.Notes)
            .HasMaxLength(2000)
            .HasComment("Additional notes");

        // Indexes for optimal query performance
        builder.HasIndex(x => x.TransactionDate)
            .HasDatabaseName("IX_GeneralLedger_TransactionDate");

        builder.HasIndex(x => x.AccountCode)
            .HasDatabaseName("IX_GeneralLedger_AccountCode");

        builder.HasIndex(x => x.PeriodId)
            .HasDatabaseName("IX_GeneralLedger_PeriodId");

        builder.HasIndex(x => x.IsPosted)
            .HasDatabaseName("IX_GeneralLedger_IsPosted");

        builder.HasIndex(x => x.Source)
            .HasDatabaseName("IX_GeneralLedger_Source");

        builder.HasIndex(x => x.ReferenceNumber)
            .HasDatabaseName("IX_GeneralLedger_ReferenceNumber");

        builder.HasIndex(x => x.PostedDate)
            .HasDatabaseName("IX_GeneralLedger_PostedDate");

        builder.HasIndex(x => x.PostedBy)
            .HasDatabaseName("IX_GeneralLedger_PostedBy");

        // Composite indexes for common query patterns
        builder.HasIndex(x => new { x.AccountCode, x.TransactionDate })
            .HasDatabaseName("IX_GeneralLedger_Account_Date");

        builder.HasIndex(x => new { x.TransactionDate, x.AccountCode })
            .HasDatabaseName("IX_GeneralLedger_Date_Account");

        builder.HasIndex(x => new { x.PeriodId, x.TransactionDate })
            .HasDatabaseName("IX_GeneralLedger_Period_Date");

        builder.HasIndex(x => new { x.IsPosted, x.TransactionDate })
            .HasDatabaseName("IX_GeneralLedger_IsPosted_Date");

        builder.HasIndex(x => new { x.Source, x.SourceId })
            .HasDatabaseName("IX_GeneralLedger_Source_SourceId");

        builder.HasIndex(x => new { x.AccountCode, x.PeriodId, x.TransactionDate })
            .HasDatabaseName("IX_GeneralLedger_Account_Period_Date");

        builder.HasIndex(x => new { x.TransactionDate, x.IsPosted, x.AccountCode })
            .HasDatabaseName("IX_GeneralLedger_Date_IsPosted_Account");

        builder.HasIndex(x => new { x.AccountCode, x.TransactionDate, x.Debit, x.Credit })
            .HasDatabaseName("IX_GeneralLedger_Account_Date_Amounts");
    }
}

