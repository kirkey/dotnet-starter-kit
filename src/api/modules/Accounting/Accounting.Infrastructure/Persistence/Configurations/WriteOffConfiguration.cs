namespace Accounting.Infrastructure.Persistence.Configurations;

public class WriteOffConfiguration : IEntityTypeConfiguration<WriteOff>
{
    public void Configure(EntityTypeBuilder<WriteOff> builder)
    {
        builder.ToTable("WriteOffs", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.ReferenceNumber).IsUnique();

        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.WriteOffDate)
            .IsRequired();

        builder.Property(x => x.WriteOffType)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.RecoveredAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.IsRecovered)
            .IsRequired();

        builder.Property(x => x.CustomerId);

        builder.Property(x => x.CustomerName)
            .HasMaxLength(256);

        builder.Property(x => x.InvoiceId);

        builder.Property(x => x.InvoiceNumber)
            .HasMaxLength(50);

        builder.Property(x => x.ReceivableAccountId)
            .IsRequired();

        builder.Property(x => x.ExpenseAccountId)
            .IsRequired();

        builder.Property(x => x.JournalEntryId);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.ApprovalStatus)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.ApprovedBy)
            .HasMaxLength(256);

        builder.Property(x => x.ApprovedDate);

        builder.Property(x => x.Reason)
            .HasMaxLength(512);

        builder.Property(x => x.Description)
            .HasMaxLength(2048);

        builder.Property(x => x.Notes)
            .HasMaxLength(2048);

        // Single column indexes
        builder.HasIndex(x => x.ReferenceNumber)
            .IsUnique()
            .HasDatabaseName("IX_WriteOffs_ReferenceNumber");
        
        builder.HasIndex(x => x.CustomerId)
            .HasDatabaseName("IX_WriteOffs_CustomerId");
        
        builder.HasIndex(x => x.InvoiceId)
            .HasDatabaseName("IX_WriteOffs_InvoiceId");
        
        builder.HasIndex(x => x.ReceivableAccountId)
            .HasDatabaseName("IX_WriteOffs_ReceivableAccountId");
        
        builder.HasIndex(x => x.ExpenseAccountId)
            .HasDatabaseName("IX_WriteOffs_ExpenseAccountId");
        
        builder.HasIndex(x => x.JournalEntryId)
            .HasDatabaseName("IX_WriteOffs_JournalEntryId");
        
        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_WriteOffs_Status");
        
        builder.HasIndex(x => x.ApprovalStatus)
            .HasDatabaseName("IX_WriteOffs_ApprovalStatus");
        
        builder.HasIndex(x => x.WriteOffDate)
            .HasDatabaseName("IX_WriteOffs_WriteOffDate");
        
        builder.HasIndex(x => x.WriteOffType)
            .HasDatabaseName("IX_WriteOffs_WriteOffType");
        
        builder.HasIndex(x => x.IsRecovered)
            .HasDatabaseName("IX_WriteOffs_IsRecovered");
        
        builder.HasIndex(x => x.ApprovedDate)
            .HasDatabaseName("IX_WriteOffs_ApprovedDate");
        
        // Composite indexes for common query patterns
        builder.HasIndex(x => new { x.Status, x.WriteOffDate })
            .HasDatabaseName("IX_WriteOffs_Status_Date");
        
        builder.HasIndex(x => new { x.ApprovalStatus, x.WriteOffDate })
            .HasDatabaseName("IX_WriteOffs_Approval_Date");
        
        builder.HasIndex(x => new { x.CustomerId, x.WriteOffDate })
            .HasDatabaseName("IX_WriteOffs_Customer_Date");
        
        builder.HasIndex(x => new { x.IsRecovered, x.WriteOffDate })
            .HasDatabaseName("IX_WriteOffs_IsRecovered_Date");
        
        // Composite index for approval workflow
        builder.HasIndex(x => new { x.ApprovalStatus, x.Status, x.WriteOffDate })
            .HasDatabaseName("IX_WriteOffs_Approval_Status_Date");
        
        // Composite index for customer write-off analysis
        builder.HasIndex(x => new { x.CustomerId, x.WriteOffType, x.Amount })
            .HasDatabaseName("IX_WriteOffs_Customer_Type_Amount");
        
        // Composite index for recovery tracking
        builder.HasIndex(x => new { x.IsRecovered, x.RecoveredAmount, x.WriteOffDate })
            .HasDatabaseName("IX_WriteOffs_Recovery_Amount_Date");
    }
}
