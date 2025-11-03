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

        builder.Property(x => x.ApprovalStatus)
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(x => x.ApprovedBy)
            .HasMaxLength(256);

        builder.Property(x => x.ApprovedDate);

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
        builder.HasIndex(x => x.PeriodId);
        builder.HasIndex(x => x.Date);
        builder.HasIndex(x => x.IsPosted);
        builder.HasIndex(x => x.Source);
        builder.HasIndex(x => x.ApprovalStatus);
    }
}
