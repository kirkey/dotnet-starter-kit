namespace Accounting.Infrastructure.Persistence.Configurations;

public class ChartOfAccountConfiguration : IEntityTypeConfiguration<ChartOfAccount>
{
    public void Configure(EntityTypeBuilder<ChartOfAccount> builder)
    {
        builder.ToTable("ChartOfAccounts", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.AccountCode).IsUnique();

        builder.Property(x => x.AccountCode)
            .HasMaxLength(16)
            .IsRequired();

        // AccountName / Name mapping
        builder.Property(x => x.AccountName)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(x => x.AccountType)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.ParentCode)
            .HasMaxLength(16);

        builder.Property(x => x.UsoaCategory)
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(x => x.Balance)
            .HasPrecision(18, 2)
            .IsRequired();

        // Additional mappings
        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.NormalBalance)
            .HasMaxLength(8);

        builder.Property(x => x.RegulatoryClassification)
            .HasMaxLength(256);

        builder.Property(x => x.Description)
            .HasMaxLength(2048);

        builder.Property(x => x.Notes)
            .HasMaxLength(2048);

        // Indexes for query optimization and foreign keys
        builder.HasIndex(x => x.AccountCode)
            .IsUnique()
            .HasDatabaseName("IX_ChartOfAccounts_AccountCode");

        builder.HasIndex(x => x.ParentCode)
            .HasDatabaseName("IX_ChartOfAccounts_ParentCode");

        builder.HasIndex(x => x.AccountType)
            .HasDatabaseName("IX_ChartOfAccounts_AccountType");

        builder.HasIndex(x => x.IsActive)
            .HasDatabaseName("IX_ChartOfAccounts_IsActive");

        builder.HasIndex(x => x.UsoaCategory)
            .HasDatabaseName("IX_ChartOfAccounts_UsoaCategory");

        builder.HasIndex(x => x.NormalBalance)
            .HasDatabaseName("IX_ChartOfAccounts_NormalBalance");

        // Composite indexes for common query patterns
        builder.HasIndex(x => new { x.AccountType, x.IsActive })
            .HasDatabaseName("IX_ChartOfAccounts_Type_IsActive");

        builder.HasIndex(x => new { x.IsActive, x.AccountType })
            .HasDatabaseName("IX_ChartOfAccounts_IsActive_Type");

        builder.HasIndex(x => new { x.ParentCode, x.AccountCode })
            .HasDatabaseName("IX_ChartOfAccounts_Parent_Code");

        builder.HasIndex(x => new { x.UsoaCategory, x.AccountType })
            .HasDatabaseName("IX_ChartOfAccounts_Usoa_Type");

        builder.HasIndex(x => new { x.AccountType, x.UsoaCategory, x.IsActive })
            .HasDatabaseName("IX_ChartOfAccounts_Type_Usoa_IsActive");
    }
}
