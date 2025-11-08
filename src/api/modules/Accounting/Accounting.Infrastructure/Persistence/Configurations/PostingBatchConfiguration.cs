namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for PostingBatch entity.
/// Configures database mapping, indexes, and constraints for batch posting operations.
/// </summary>
public class PostingBatchConfiguration : IEntityTypeConfiguration<PostingBatch>
{
    public void Configure(EntityTypeBuilder<PostingBatch> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(nameof(PostingBatch), SchemaNames.Accounting);

        builder.HasKey(p => p.Id);

        // Properties
        builder.Property(p => p.BatchNumber)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Unique batch number for tracking");

        builder.Property(p => p.Description)
            .HasMaxLength(500)
            .HasComment("Description of the posting batch");

        builder.Property(p => p.PostingDate)
            .IsRequired()
            .HasComment("Date when the batch is posted to general ledger");

        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Batch status: Draft, Posted, Reversed");

        builder.Property(p => p.TotalDebits)
            .IsRequired()
            .HasPrecision(16, 2)
            .HasComment("Total debit amount in the batch");

        builder.Property(p => p.TotalCredits)
            .IsRequired()
            .HasPrecision(16, 2)
            .HasComment("Total credit amount in the batch");

        builder.Property(p => p.EntryCount)
            .IsRequired()
            .HasComment("Number of journal entries in the batch");

        builder.Property(p => p.PostedBy)
            .HasMaxLength(256)
            .HasComment("User who posted the batch");

        builder.Property(p => p.PostedOn)
            .HasComment("Timestamp when the batch was posted");

        builder.Property(p => p.ReversedOn)
            .HasComment("Timestamp when the batch was reversed");

        builder.Property(p => p.ReversedBy)
            .HasMaxLength(256)
            .HasComment("User who reversed the batch");

        builder.Property(p => p.Notes)
            .HasMaxLength(2000)
            .HasComment("Additional notes about the batch");

        // Indexes for optimal query performance
        builder.HasIndex(p => p.BatchNumber)
            .IsUnique()
            .HasDatabaseName("IX_PostingBatch_BatchNumber");

        builder.HasIndex(p => p.Status)
            .HasDatabaseName("IX_PostingBatch_Status");

        builder.HasIndex(p => p.PostingDate)
            .HasDatabaseName("IX_PostingBatch_PostingDate");

        builder.HasIndex(p => p.PostedBy)
            .HasDatabaseName("IX_PostingBatch_PostedBy");

        builder.HasIndex(p => p.PostedOn)
            .HasDatabaseName("IX_PostingBatch_PostedOn");

        // Composite indexes for common query patterns
        builder.HasIndex(p => new { p.Status, p.PostingDate })
            .HasDatabaseName("IX_PostingBatch_Status_PostingDate");

        builder.HasIndex(p => new { p.PostingDate, p.Status, p.BatchNumber })
            .HasDatabaseName("IX_PostingBatch_PostingDate_Status_BatchNumber");

        builder.HasIndex(p => new { p.PostedBy, p.PostedOn })
            .HasDatabaseName("IX_PostingBatch_PostedBy_PostedOn");

        builder.HasIndex(p => new { p.Status, p.TotalDebits, p.TotalCredits })
            .HasDatabaseName("IX_PostingBatch_Status_Amounts");
    }
}

