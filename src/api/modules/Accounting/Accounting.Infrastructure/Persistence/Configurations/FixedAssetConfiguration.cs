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

        // Indexes for foreign keys and query optimization
        builder.HasIndex(x => x.DepreciationMethodId);
        builder.HasIndex(x => x.AccumulatedDepreciationAccountId);
        builder.HasIndex(x => x.DepreciationExpenseAccountId);
        builder.HasIndex(x => x.IsDisposed);
        builder.HasIndex(x => x.PurchaseDate);
    }
}
