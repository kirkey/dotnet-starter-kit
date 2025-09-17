namespace Accounting.Infrastructure.Persistence.Configurations;

public class JournalEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
{
    public void Configure(EntityTypeBuilder<JournalEntry> builder)
    {
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

        // Configure owned entity for JournalEntryLines
        builder.OwnsMany(x => x.Lines, jel =>
        {
            jel.ToTable("JournalEntryLines", schema: SchemaNames.Accounting);
            jel.WithOwner().HasForeignKey("JournalEntryId");
            jel.HasKey("Id");
            
            jel.Property(x => x.AccountId)
                .IsRequired();
                
            jel.Property(x => x.DebitAmount)
                .HasPrecision(18, 2)
                .IsRequired();
                
            jel.Property(x => x.CreditAmount)
                .HasPrecision(18, 2)
                .IsRequired();
        });
    }
}
