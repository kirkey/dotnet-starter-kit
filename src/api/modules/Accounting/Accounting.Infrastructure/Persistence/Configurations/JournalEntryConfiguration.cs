using Finbuckle.MultiTenant;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class JournalEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
{
    public void Configure(EntityTypeBuilder<JournalEntry> builder)
    {
        builder.IsMultiTenant();
        
        builder.ToTable("JournalEntries", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.ReferenceNumber).IsUnique();

        builder.Property(x => x.Date)
            .IsRequired();

        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.Source)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.IsPosted)
            .IsRequired();

        builder.Property(x => x.PeriodId);

        builder.Property(x => x.OriginalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        // Configure relationship to journal entry lines (one-to-many)
        builder.HasMany(x => x.Lines)
            .WithOne()
            .HasForeignKey(l => l.JournalEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ensure EF uses the backing field for change tracking
        var navigation = builder.Metadata.FindNavigation(nameof(JournalEntry.Lines));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        // Note: Property-level configuration for JournalEntryLine is handled in JournalEntryLineConfiguration

        // Indexes for foreign keys and query optimization
        builder.HasIndex(x => x.ReferenceNumber)
            .IsUnique()
            .HasDatabaseName("IX_JournalEntries_ReferenceNumber");

        builder.HasIndex(x => x.PeriodId)
            .HasDatabaseName("IX_JournalEntries_PeriodId");

        builder.HasIndex(x => x.Date)
            .HasDatabaseName("IX_JournalEntries_Date");

        builder.HasIndex(x => x.IsPosted)
            .HasDatabaseName("IX_JournalEntries_IsPosted");

        builder.HasIndex(x => x.Source)
            .HasDatabaseName("IX_JournalEntries_Source");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_JournalEntries_Status");

        // Composite indexes for common query patterns
        builder.HasIndex(x => new { x.Date, x.IsPosted })
            .HasDatabaseName("IX_JournalEntries_Date_IsPosted");

        builder.HasIndex(x => new { x.IsPosted, x.Date })
            .HasDatabaseName("IX_JournalEntries_IsPosted_Date");

        builder.HasIndex(x => new { x.Source, x.Date })
            .HasDatabaseName("IX_JournalEntries_Source_Date");

        builder.HasIndex(x => new { x.Status, x.Date })
            .HasDatabaseName("IX_JournalEntries_Status_Date");

        builder.HasIndex(x => new { x.PeriodId, x.Date, x.IsPosted })
            .HasDatabaseName("IX_JournalEntries_Period_Date_IsPosted");

        builder.HasIndex(x => new { x.Date, x.Source, x.IsPosted })
            .HasDatabaseName("IX_JournalEntries_Date_Source_IsPosted");
    }
}
