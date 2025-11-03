using Finbuckle.MultiTenant;

namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for JournalEntryLine entity.
/// </summary>
public class JournalEntryLineConfiguration : IEntityTypeConfiguration<JournalEntryLine>
{
    public void Configure(EntityTypeBuilder<JournalEntryLine> builder)
    {
        builder.IsMultiTenant();
        
        builder.ToTable("JournalEntryLines", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.JournalEntryId)
            .IsRequired();

        builder.Property(x => x.AccountId)
            .IsRequired();

        builder.Property(x => x.DebitAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.CreditAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Memo)
            .HasMaxLength(500);

        builder.Property(x => x.Reference)
            .HasMaxLength(100);

        // Indexes for foreign keys and query optimization
        builder.HasIndex(x => x.JournalEntryId);
        builder.HasIndex(x => x.AccountId);
        builder.HasIndex(x => x.Reference);

        // Composite index for querying lines by entry and account
        builder.HasIndex(x => new { x.JournalEntryId, x.AccountId })
            .HasDatabaseName("IX_JournalEntryLines_Entry_Account");

        // Note: Relationship to JournalEntry is configured from JournalEntryConfiguration.HasMany(...).WithOne()
    }
}

