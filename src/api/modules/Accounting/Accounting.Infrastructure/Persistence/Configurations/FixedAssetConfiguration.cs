namespace Accounting.Infrastructure.Persistence.Configurations;

public class FixedAssetConfiguration : IEntityTypeConfiguration<FixedAsset>
{
    public void Configure(EntityTypeBuilder<FixedAsset> builder)
    {
        builder.ToTable("FixedAssets", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PurchaseDate)
            .IsRequired();

        builder.Property(x => x.PurchasePrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.ServiceLife)
            .IsRequired();

        builder.Property(x => x.DepreciationMethodId)
            .IsRequired();

        builder.Property(x => x.SalvageValue)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.CurrentBookValue)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.AccumulatedDepreciationAccountId)
            .IsRequired();

        builder.Property(x => x.DepreciationExpenseAccountId)
            .IsRequired();

        builder.Property(x => x.SerialNumber)
            .HasMaxLength(100);

        builder.Property(x => x.Location)
            .HasMaxLength(256);

        builder.Property(x => x.Department)
            .HasMaxLength(100);

        builder.Property(x => x.IsDisposed)
            .IsRequired();

        builder.Property(x => x.DisposalDate);

        builder.Property(x => x.DisposalAmount)
            .HasPrecision(18, 2);

        // Single column indexes
        builder.HasIndex(x => x.DepreciationMethodId)
            .HasDatabaseName("IX_FixedAssets_DepreciationMethodId");
        
        builder.HasIndex(x => x.AccumulatedDepreciationAccountId)
            .HasDatabaseName("IX_FixedAssets_AccumulatedDepreciationAccountId");
        
        builder.HasIndex(x => x.DepreciationExpenseAccountId)
            .HasDatabaseName("IX_FixedAssets_DepreciationExpenseAccountId");
        
        builder.HasIndex(x => x.IsDisposed)
            .HasDatabaseName("IX_FixedAssets_IsDisposed");
        
        builder.HasIndex(x => x.PurchaseDate)
            .HasDatabaseName("IX_FixedAssets_PurchaseDate");
        
        builder.HasIndex(x => x.DisposalDate)
            .HasDatabaseName("IX_FixedAssets_DisposalDate");
        
        builder.HasIndex(x => x.SerialNumber)
            .HasDatabaseName("IX_FixedAssets_SerialNumber");
        
        builder.HasIndex(x => x.Location)
            .HasDatabaseName("IX_FixedAssets_Location");
        
        builder.HasIndex(x => x.Department)
            .HasDatabaseName("IX_FixedAssets_Department");
        
        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_FixedAssets_Status");
        
        // Composite indexes for common query patterns
        builder.HasIndex(x => new { x.IsDisposed, x.PurchaseDate })
            .HasDatabaseName("IX_FixedAssets_IsDisposed_PurchaseDate");
        
        builder.HasIndex(x => new { x.Location, x.IsDisposed })
            .HasDatabaseName("IX_FixedAssets_Location_IsDisposed");
        
        builder.HasIndex(x => new { x.Department, x.IsDisposed })
            .HasDatabaseName("IX_FixedAssets_Department_IsDisposed");
        
        // Composite index for depreciation reporting
        builder.HasIndex(x => new { x.DepreciationMethodId, x.IsDisposed, x.PurchaseDate })
            .HasDatabaseName("IX_FixedAssets_Method_IsDisposed_Purchase");
        
        // Composite index for book value queries
        builder.HasIndex(x => new { x.IsDisposed, x.CurrentBookValue })
            .HasDatabaseName("IX_FixedAssets_IsDisposed_BookValue");
        
        // Composite index for disposal tracking
        builder.HasIndex(x => new { x.IsDisposed, x.DisposalDate, x.DisposalAmount })
            .HasDatabaseName("IX_FixedAssets_Disposal_Date_Amount");
    }
}
