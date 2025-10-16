namespace Accounting.Infrastructure.Persistence.Configurations;

public class RecurringJournalEntryConfiguration : IEntityTypeConfiguration<RecurringJournalEntry>
{
    public void Configure(EntityTypeBuilder<RecurringJournalEntry> builder)
    {
        builder.ToTable("RecurringJournalEntries", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.TemplateCode).IsUnique();

        builder.Property(x => x.TemplateCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(x => x.Frequency)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.CustomIntervalDays);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.DebitAccountId)
            .IsRequired();

        builder.Property(x => x.CreditAccountId)
            .IsRequired();

        builder.Property(x => x.StartDate)
            .IsRequired();

        builder.Property(x => x.EndDate);

        builder.Property(x => x.NextRunDate)
            .IsRequired();

        builder.Property(x => x.LastGeneratedDate);

        builder.Property(x => x.GeneratedCount)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.ApprovedBy)
            .HasMaxLength(256);

        builder.Property(x => x.ApprovedDate);

        builder.Property(x => x.PostingBatchId);

        builder.Property(x => x.Memo)
            .HasMaxLength(512);

        builder.Property(x => x.Notes)
            .HasMaxLength(2048);
    }
}
